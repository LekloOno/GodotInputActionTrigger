namespace GIAT.Components.Input.Handler;

using Godot;

using GIAT.Interface;
using GIAT.Components.Trigger;
using GIAT.Components.Input.Buffer;
using Godot.Collections;

[Tool]
public abstract partial class InputHandler<T> : NodeTrigger<T>, IAction, IAction<T> where T: IInput
{
    /// <summary>
    /// Make the input trigger its actions autonomously.
    /// </summary>
    private bool _autonomous = true;
    /// <summary>
    /// Makes so only successfull call to Do() consume used buffer input.
    /// </summary>
    [Export]
    private bool _consumeSuccessOnly = true;

    private bool IsTrigger => _autonomous && _rootTrigger;

    private bool _rootTrigger = false;

    public ulong LastInputStamp {get; private set;}
    private IBuffer<T> _buffer;
    
    public BufferData<T> _bufferData;
    public BufferData<T> BufferData
    {
        get => _bufferData;
        protected set
        {
            if (_bufferData == value)
                return;

            _bufferData = value;
            
            if (value == null)
                _buffer = new EmptyBuffer<T>();
            else
                _buffer = value.Build();
        }
    }

    public override void _Ready()
    {
        GD.Print(IsTrigger);
    }

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
            _buffer.Clear(input);

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
        DoSpec(input);
        LastInputStamp = PHX_Time.ScaledTicksMsec;
        bool handled = IsTrigger && TriggerActions(input);

        if (handled)
            return true;

        _buffer.Buffer(input);
        return false;
    }

    public abstract void DoSpec(T input);

    protected override void CheckParentSpec()
        => _rootTrigger = GetParent() is not ITrigger;

    protected override void UnparentSpec()
        => _rootTrigger = true;


    // Inspector
    public override Array<Dictionary> _GetPropertyList()
    {
        Array<Dictionary> properties = [];
        
        if (GetParent() is not ITrigger)
        {
            properties.Add(new Dictionary
            {
                { "name", "_autonomous" },
                { "type", (int)Variant.Type.Bool },
                { "usage", (int)PropertyUsageFlags.Default }
            });
        }

        return properties;
    }

    public override bool _PropertyCanRevert(StringName property)
    {
        if (property == "_autonomous")
            return _autonomous != true;

        return false;
    }

    public override Variant _PropertyGetRevert(StringName property)
    {
        if (property == "_autonomous")
            return true;

        return default;
    }
}