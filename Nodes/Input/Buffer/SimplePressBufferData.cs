namespace GIAT.Nodes.Input.Buffer;

using Godot;

using GIAT.Interface;
using GIAT.Components.Input.Buffer;
using GIAT.Nodes.Input.Type;

[GlobalClass, Tool]
public partial class SimplePressBufferData : PressBufferData, SimpleBufferData<PressInput>
{
    [Export] public ulong LifeTime {get; private set;}

    [Export(PropertyHint.Flags)]
    public PressState Buffered {get; private set;} = PressState.Start | PressState.Stop;

    public override IBuffer<PressInput> Build()
        => new SimplePressBuffer(this);
}