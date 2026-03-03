namespace GIAT.Nodes.Input.Producer;

using Godot;

using GIAT.Nodes.Input.Type;
using GIAT.Components.Input.Producer;

public abstract class PressInputProducer : Producer<PressInput>
{
    public ulong LastInputStart {get; protected set;}
    public ulong LastInputStop {get; protected set;}
    public bool Active {get; protected set;}

    public override bool ProduceInternal(InputEvent @event, out PressInput input)
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

    protected PressInput ProduceStart()
    {
        LastInputStart = Time.GetTicksMsec();
        Active = true;
        return PressInput.Start;
    }

    protected PressInput ProduceStop()
    {
        LastInputStop = Time.GetTicksMsec();
        Active = false;
        return PressInput.Stop;
    }

    protected abstract bool ProcessPressed(out PressInput input);
    protected abstract bool ProcessReleased(out PressInput input);
}