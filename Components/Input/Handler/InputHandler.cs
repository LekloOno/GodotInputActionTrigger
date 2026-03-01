namespace GIAT.Components.Input.Handler;

using Godot;

using GIAT.Interface;
using GIAT.Components.Trigger;
using GIAT.Components.Input.Buffer;
using Godot.Collections;
using GIAT.Nodes.Input.Handler;

[Tool]
public abstract partial class InputHandler<T> : NodeTrigger<T>, IAction, IAction<T>
{
    protected string _actionName = "";
    /// <summary>
    /// Makes so the input handler consumes its own inputs.
    /// </summary>
    private bool _consume = true;
    /// <summary>
    /// Makes so only successfull call to Do() consume used buffer input.
    /// </summary>
    [Export]
    private bool _consumeSuccessOnly = true;
    /// <summary>
    /// Makes so the input handler produces its own inputs
    /// </summary>
    private bool _produce = true;

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
        => SetProcessUnhandledInput(_produce && _enabled && _producer != null);

    private bool IsTrigger => _consume && _rootTrigger;

    private bool _rootTrigger = false;

    public ulong LastInputStamp {get; private set;}
    protected IInputProducer<T> _producer;
    
    private IBuffer<T> _buffer = new EmptyBuffer<T>();
    
    public BufferData _bufferData;

    protected virtual IBuffer<T> Build()
        => _bufferData.Build<T>();

    private bool TriggerActions(T input)
    {
        foreach (IAction<T> action in _actions)
        {
            bool handled;

            //if (action is IAction signal)
            //    handled = signal.Do();
            //else
                handled = action.Do(input);
            
            if (handled)
                return true;
        }
        
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

    public bool Do(T input)
    {
        if (!_producer.ProduceExternal(input))
            return false;

        LastInputStamp = PHX_Time.ScaledTicksMsec;
        // Buffer in preamble, to ensure a consistent
        // produce - consume semantic.
        if (!_buffer.Buffer(input))
            return false;
        return Do();
    }

    protected override void CheckParentSpec()
        => _rootTrigger = GetParent() is not ITrigger;

    protected override void UnparentSpec()
        => _rootTrigger = true;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsAction(_actionName))
            return;

        if (!_producer.Produce(@event, out T input))
            return;
        
        if (!_buffer.Buffer(input))
            return;
        
        if (IsTrigger)
            Do();
    }

    public override void _Ready()
    {
        SetProcessUnhandledInput();
    }

    protected override void EnterTreeSpec()
    {
        SetProcessUnhandledInput();   
    }
}