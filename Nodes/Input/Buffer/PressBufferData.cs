namespace GIAT.Nodes.Input.Buffer;

using Godot;

using GIAT.Nodes.Input.Type;

[GlobalClass, Tool]
public partial class PressBufferData : Resource
{
    [Export(PropertyHint.Flags)]
    public PressState Buffered {get; private set;} = PressState.Start | PressState.Stop;
}