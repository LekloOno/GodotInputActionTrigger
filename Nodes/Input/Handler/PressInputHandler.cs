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
        get
        {
            if (BufferData is PressBufferData buf)
                return buf;
            return null;   
        }
        set => BufferData = value;
    }

    public ulong LastInputStart {get; private set;}
    public ulong LastInputStop {get; private set;}
    public bool Active {get; private set;}

    public override void DoSpec(PressInput input)
    {
        switch (input.state)
        {
            case PressState.Start:
                LastInputStart = PHX_Time.ScaledTicksMsec;
                Active = true;
                break;
            case PressState.Stop:
                LastInputStop = PHX_Time.ScaledTicksMsec;
                Active = false;
                break;
        }
    }

    protected override void CheckChildrenSpec(){}
}