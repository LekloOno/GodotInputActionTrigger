namespace GIAT.Components.Input.Handler;

using Godot;

using GIAT.Interface;
using GIAT.Components.Trigger;
using GIAT.Components.Input.Buffer;

[Tool]
public abstract partial class InputHandler<T> : NodeTrigger<T>, IAction, IProducer<T>
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

    private bool _rootTrigger = false;

    public ulong LastInputStamp {get; private set;}
    protected IProducer<T> _producer;
    
    private IBuffer<T> _buffer = new EmptyBuffer<T>();
    
    public BufferData _bufferData;

    protected virtual IBuffer<T> Build()
        => _bufferData.Build<T>();

    private bool TriggerActions(T input)
    {
        foreach (IAction<T> action in _actions)
            if (action.Do(input))
                return true;
        
        return false;
    }

    private bool SuccessDo()
    {
        if (!_buffer.Peak(out T input))
            return false;

        bool handled = TriggerActions(input);

        if (handled)
            _buffer.Pop();

        return handled;
    }

    private bool SimpleDo()
    {
        if (!_buffer.Consume(out T input))
            return false;

        return TriggerActions(input);
    }

    public bool Do()
    {
        if (_consumeSuccessOnly)
            return SuccessDo();

        return SimpleDo();
    }

    protected override void CheckParentSpec()
        => _rootTrigger = GetParent() is not ITrigger;

    protected override void UnparentSpec()
        => _rootTrigger = true;

    public override void _UnhandledInput(InputEvent @event)
        => Produce(@event, out _);

    public override void _Ready()
    {
        SetProcessUnhandledInput();
    }

    protected override void EnterTreeSpec()
    {
        SetProcessUnhandledInput();   
    }

    public bool Produce(InputEvent @event, out T input)
    {
        input = default;
        if (!@event.IsAction(_actionName))
            return false;

        if (!_producer.Produce(@event, out input))
            return false;
        
        if (!_buffer.Buffer(input))
            return false;
        
        if (IsTrigger)
            Do();
        
        return true;
    }

    public bool ProduceExternal(T input)
    {
        if (!_producer.ProduceExternal(input))
            return false;

        LastInputStamp = PHX_Time.ScaledTicksMsec;
        // Buffer in preamble, to ensure a consistent
        // produce - consume semantic.
        if (!_buffer.Buffer(input))
            return false;

        if (IsTrigger)
            Do();

        return true;
    }
}