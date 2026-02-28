namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;
using Godot;
using Godot.Collections;

[GlobalClass, Tool]
public partial class SimpleBufferData : BufferData
{
    private bool _useLifeTime = false;

    public ulong LifeTime {get; private set;} = 20;

    public override IBuffer<T> Build<T>()
        => new SimpleBuffer<T>(this);

    // +---------------------+
    // |   INSPECTOR VIEW    |
    // +---------------------+
    // _______________________
    [Export]
    public bool UseLifeTime
    {
        get => _useLifeTime;
        private set
        {
            if (value == _useLifeTime)
                return;

            _useLifeTime = value;
            NotifyPropertyListChanged();       
        }
    }

    public override Array<Dictionary> _GetPropertyList()
    {
        Array<Dictionary> properties = [];
        
        if (UseLifeTime)
        {
            properties.Add(new Dictionary
            {
                { "name", "LifeTime" },
                { "type", (int)Variant.Type.Int },
                { "usage", (int)PropertyUsageFlags.Default }
            });
        }

        return properties;
    }

    public override bool _PropertyCanRevert(StringName property)
    {
        if (property == "LifeTime")
            return LifeTime != 20;

        return false;
    }

    public override Variant _PropertyGetRevert(StringName property)
    {
        if (property == "LifeTime")
            return 20;

        return default;
    }
}