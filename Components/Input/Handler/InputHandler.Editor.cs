namespace GIAT.Components.Input.Handler;

using Godot;

using GIAT.Interface;
using Godot.Collections;
using GIAT.Components.Input.Buffer;

public partial class InputHandler<T>
{
    /// <summary>
    /// Makes so the input handler produces its own inputs
    /// </summary>
    [Export]
    public bool Produce
    {
        get => _produce;
        set
        {
            _produce = value;
            SetProcessUnhandledInput();
            NotifyPropertyListChanged();
        }
    }
    /// <summary>
    /// It is named buffer although it is the buffer data because the buffer itself
    /// is not exposed to the editor. <br/>
    /// <br/>
    /// It implictly represents the buffer for the editor-land.
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
    
    public override Array<Dictionary> _GetPropertyList()
    {
        Array<Dictionary> properties = [];
        
        if (_produce)
            properties.Add(InputActionProperty());

        if (GetParent() is not ITrigger)
            properties.Add(AutonomousProperty());


        return properties;
    }

    private Dictionary AutonomousProperty()
        => new Dictionary
            {
                { "name", nameof(_consume) },
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
        if (property == nameof(_consume))
            return _consume != true;

        return false;
    }

    public override Variant _PropertyGetRevert(StringName property)
    {
        if (property == nameof(_consume))
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