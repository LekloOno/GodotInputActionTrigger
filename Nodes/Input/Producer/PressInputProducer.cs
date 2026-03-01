namespace GIAT.Nodes.Input.Producer;

using Godot;

using GIAT.Nodes.Input.Type;
using GIAT.Interface;

public abstract class PressInputProducer : IProducer<PressInput>
{
    public ulong LastInputStart {get; protected set;}
    public ulong LastInputStop {get; protected set;}
    public bool Active {get; protected set;}

    public bool Produce(InputEvent @event, out PressInput input)
    {
        input = default;
        if (@event.IsEcho())
            return false;

        if (@event.IsPressed())
            return ProcessPressed(out input);
                
        if (@event.IsReleased())
            return ProcessReleased(out input);

        return false;
    }

    public bool ProduceExternal(PressInput input)
    {
        return input switch
        {
            PressInput.Start => ProduceExternalStart(),
            PressInput.Stop => ProduceExternalStop(),
            _ => false,
        };
    }

    private bool ProduceExternalStart()
    {
        if (Active)
            return false;

        StampStart();
        return true;
    }

    private bool ProduceExternalStop()
    {
        if (!Active)
            return false;

        StampStop();
        return true;
    }

    private void StampStart()
    {
        LastInputStart = PHX_Time.ScaledTicksMsec;
        Active = true;
    }

    private void StampStop()
    {
        LastInputStop = PHX_Time.ScaledTicksMsec;
        Active = false;
    }

    protected PressInput ProduceStart()
    {
        StampStart();
        return PressInput.Start;
    }

    protected PressInput ProduceStop()
    {
        StampStop();
        return PressInput.Stop;
    }

    protected abstract bool ProcessPressed(out PressInput input);
    protected abstract bool ProcessReleased(out PressInput input);
}