namespace GIAT.Nodes.Input.Producer;

using GIAT.Nodes.Input.Type;

public class TapProducer : PressInputProducer
{
    protected override bool ProcessPressed(out PressInput input)
    {
        if (Active)
            input = ProduceStop();
        else
            input = ProduceStart();
        return true;
    }

    protected override bool ProcessReleased(out PressInput input)
    {
        input = default;
        return false;
    }
}