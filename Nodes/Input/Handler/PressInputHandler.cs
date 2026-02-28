namespace GIAT.Nodes.Input.Handler;

using Godot;

using GIAT.Components.Input.Handler;
using GIAT.Nodes.Input.Type;
using GIAT.Nodes.Input.Buffer;
using GIAT.Interface;
using GIAT.Components.Input.Buffer;

[GlobalClass, Tool]
public partial class PressInputHandler : InputHandler<PressInput>
{
    [Export] public PressBufferData PressBufferData = new();

    public ulong LastInputStart {get; private set;}
    public ulong LastInputStop {get; private set;}
    public bool Active {get; private set;}

    protected override IBuffer<PressInput> Build()
        => new PressBuffer(BufferData.Build<PressInput>(), PressBufferData);

    public override void DoSpec(PressInput input)
    {
        switch (input)
        {
            case PressInput.Start:
                LastInputStart = PHX_Time.ScaledTicksMsec;
                Active = true;
                break;
            case PressInput.Stop:
                LastInputStop = PHX_Time.ScaledTicksMsec;
                Active = false;
                break;
        }
    }

    protected override void CheckChildrenSpec(){}
}