namespace GIAT.Nodes.Input.Producer;

using Godot;

using GIAT.Nodes.Input.Type;
using GIAT.Interface;

public abstract class PressInputProducer : IInputProducer<PressInput>
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
            PressInput.Start => ProcessPressed(out _),
            PressInput.Stop => ProcessReleased(out _),
            _ => false,
        };
    }

    protected PressInput ProduceStart()
    {
        LastInputStart = PHX_Time.ScaledTicksMsec;
        Active = true;
        return PressInput.Start;
    }

    protected PressInput ProduceStop()
    {
        LastInputStop = PHX_Time.ScaledTicksMsec;
        Active = false;
        return PressInput.Stop;
    }

    protected abstract bool ProcessPressed(out PressInput input);
    protected abstract bool ProcessReleased(out PressInput input);
}