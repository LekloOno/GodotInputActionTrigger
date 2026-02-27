namespace GIAT.Nodes.Input.Buffer;

using Godot;

using GIAT.Interface;
using GIAT.Components.Input.Buffer;
using GIAT.Nodes.Input.Type;

[GlobalClass]
public partial class SimplePressBufferData : PressBufferData, SimpleBufferData<PressInput>
{
    [Export] public ulong LifeTime {get; private set;}
    [Export] public bool IgnoreStop {get; private set;}

    public override IBuffer<PressInput> Build()
        => new SimplePressBuffer(this);
}