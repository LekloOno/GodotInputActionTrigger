namespace GIAT.Components.Input.Handler;

using Godot;

using GIAT.Interface;
using GIAT.Components.Trigger;
using GIAT.Components.Input.Buffer;

[Tool]
public abstract partial class InputHandler<T> : NodeTrigger<T>, IAction, IAction<InputEvent>
{
    protected string _actionName = "";
    /// <summary>
    /// Makes so the input handler consumes its own inputs.
    /// </summary>
    private bool _consumeSelf = true;
    /// <summary>
    /// Makes so only successfull call to Do() consume used buffer input.
    /// </summary>
    [Export]
    private bool _consumeSuccessOnly = true;
    /// <summary>
    /// Makes so the input handler produces its own inputs
    /// </summary>
    private bool _produceSelf = true;

    private bool _enabled = true;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            SetProcessUnhandledInput();   
        }
    }

    private void SetProcessUnhandledInput()
        => SetProcessUnhandledInput(_produceSelf && _enabled && _producer != null);

    private bool IsTrigger => _consumeSelf && _rootTrigger;
    public ulong LastInputStamp => _producer.LastInputStamp;
    private bool _rootTrigger = false;

    protected IProducer<T> _producer;
    public BufferData _bufferData;
    private IBuffer<T> _buffer = new EmptyBuffer<T>();

    protected virtual IBuffer<T> Build()
        => _bufferData.Build<T>();

    protected override void CheckParentSpec()
        => _rootTrigger = GetParent() is not ITransmitter;

    protected override void UnparentSpec()
        => _rootTrigger = true;

    public override void _UnhandledInput(InputEvent @event)
        => Do(@event);

    public override void _Ready()
        => SetProcessUnhandledInput();

    protected override void EnterTreeSpec()
        => SetProcessUnhandledInput();

    public bool Do(InputEvent @event)
    {
        if (!@event.IsAction(_actionName))
            return false;

        if(!_produceSelf)
            return false;

        if (_producer == null)
            return false;

        if(!_producer.Produce(@event, out T input))
            return false;

        if (!IsTrigger || !Do(input))
            return false;

        //GetViewport().SetInputAsHandled();
        // This is linked to the problem mentionned below.
        return true;
    }

    public override bool Do(T input)
    {
        if (Transmit(input))
            return true;

        // Problem spotted :
        // Buffer might need to be called from the exterior.
        // If an action fails, it will instantly try to buffer input,
        // although one of its siblings actions might have succeeded.
        // We should buffer if none did, but then where ?
        // In the parent ? then we moved the problem up

        // Gotta figure this out !
        if (_consumeSuccessOnly)
            return _buffer.Buffer(input);

        return true;
    }

    public bool Do()
    {
        if (!_buffer.Consume(out T input))
            return false;

        return Do(input);
    }
}