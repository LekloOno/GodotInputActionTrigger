namespace GIAT.Nodes.Input.Buffer;

using Godot;

using GIAT.Interface;
using GIAT.Components.Input.Buffer;
using GIAT.Nodes.Input.Type;
using Godot.Collections;

[GlobalClass, Tool]
public partial class SimplePressBufferData : PressBufferData, SimpleBufferData<PressInput>
{
    [Export(PropertyHint.Flags)]
    public PressState Buffered {get; private set;} = PressState.Start | PressState.Stop;

    private bool _useLifeTime = false;

    public ulong LifeTime {get; private set;} = 20;

    public override IBuffer<PressInput> Build()
        => new SimplePressBuffer(this);


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