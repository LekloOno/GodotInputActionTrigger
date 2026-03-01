namespace GIAT.Nodes.Input.Buffer;

using Godot;

using GIAT.Nodes.Input.Type;
using Godot.Collections;

public partial class PressBufferData : Resource
{
    [Export(PropertyHint.Flags)]
    public PressInput Buffered
    {
        get => _buffered;
        set
        {
            if (value == _buffered)
                return;
            
            _buffered = value;
            NotifyPropertyListChanged();
        }
    }

    public override Array<Dictionary> _GetPropertyList()
    {
        if (!_cancelAvailable)
            return base._GetPropertyList();

        Array<Dictionary> properties = [];
        properties.Add(OtherCancelsProperty());

        return properties;
    }

    private Dictionary OtherCancelsProperty()
        => new Dictionary
            {
                { "name", nameof(_otherCancels) },
                { "type", (int)Variant.Type.Bool },
                { "usage", (int)PropertyUsageFlags.Default }
            };
}