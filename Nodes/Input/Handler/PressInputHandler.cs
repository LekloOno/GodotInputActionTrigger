namespace GIAT.Nodes.Input.Handler;

using Godot;

using GIAT.Components.Input.Handler;
using GIAT.Nodes.Input.Type;
using GIAT.Nodes.Input.Buffer;

[GlobalClass, Tool]
public partial class PressInputHandler : InputHandler<PressInput>
{
    [Export] public PressBufferData PressBufferData
    {
        get => (PressBufferData)BufferData;
        set => BufferData = value;
    }

    protected override void CheckChildrenSpec(){}
}