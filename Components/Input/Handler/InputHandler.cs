namespace GIAT.Components.Input.Handler;

using Godot;

using GIAT.Interface;
using GIAT.Components.Trigger;
using GIAT.Components.Input.Buffer;
using Godot.Collections;
using System.Linq;

[Tool]
public abstract partial class InputHandler<T> : NodeTrigger<T>, IAction, IAction<T>
{
    protected string _actionName = "";
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
    private IBuffer<T> _buffer = new EmptyBuffer<T>();
    
    public BufferData _bufferData;

    /// <summary>
    /// It is named buffer although it is the buffer data because the buffer itself
    /// is not exposed to the editor. <br/>
    /// <br/>
    /// It implictly represents the buffer for the inspector-land.
    /// </summary>
    [Export]
    public BufferData Buffer
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
                _buffer = Build();
        }
    }

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

    public bool Do(T input)
    {
        DoSpec(input);
        LastInputStamp = PHX_Time.ScaledTicksMsec;
        // Buffer in preamble, to ensure a consistent
        // produce - consume semantic.
        _buffer.Buffer(input);
        bool handled = IsTrigger && TriggerActions(input);

        if (!handled)
            return false;

        _buffer.Pop();
        return true;
    }

    public abstract void DoSpec(T input);

    protected override void CheckParentSpec()
        => _rootTrigger = GetParent() is not ITrigger;

    protected override void UnparentSpec()
        => _rootTrigger = true;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction(_actionName))
            Process(@event);
    }

    protected abstract void Process(InputEvent @event);


    // +---------------------+
    // |   INSPECTOR VIEW    |
    // +---------------------+
    // _______________________
    public override Array<Dictionary> _GetPropertyList()
    {
        Array<Dictionary> properties = [];
        
        properties.Add(InputActionProperty());

        if (GetParent() is not ITrigger)
            properties.Add(AutonomousProperty());


        return properties;
    }

    private Dictionary AutonomousProperty()
        => new Dictionary
            {
                { "name", "_autonomous" },
                { "type", (int)Variant.Type.Bool },
                { "usage", (int)PropertyUsageFlags.Default }
            };

    private Dictionary InputActionProperty()
    {
        ConfigFile config = new();
        config.Load("res://project.godot");

        var actions = config.GetSectionKeys("input");

        // Convert to comma-separated string for enum hint
        string hintString = string.Join(",", actions);

        return new Dictionary
        {
            { "name", nameof(_actionName) },
            { "type", (int)Variant.Type.String },
            { "usage", (int)PropertyUsageFlags.Default },
            { "hint", (int)PropertyHint.Enum },
            { "hint_string", hintString }
        };
    } 

    public override bool _PropertyCanRevert(StringName property)
    {
        if (property == nameof(_autonomous))
            return _autonomous != true;

        return false;
    }

    public override Variant _PropertyGetRevert(StringName property)
    {
        if (property == nameof(_autonomous))
            return true;

        return default;
    }

    public override Variant _Get(StringName property)
    {
        if (property == nameof(_actionName))
            return _actionName;

        return default;
    }

    public override bool _Set(StringName property, Variant value)
    {
        if (property == nameof(_actionName))
        {
            _actionName = value.AsString();
            return true;
        }

        return false;
    }
}