namespace GIAT.Nodes.Input.Buffer;

using Godot;

using GIAT.Interface;
using GIAT.Components.Input.Buffer;
using GIAT.Nodes.Input.Type;

[GlobalClass]
public abstract partial class PressBufferData : Resource, BufferData<PressInput>
{
    public abstract IBuffer<PressInput> Build();
}