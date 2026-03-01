namespace GIAT.Nodes.Input.Producer;

using GIAT.Nodes.Input.Type;

public class HoldProducer : PressInputProducer
{
    protected override bool ProcessPressed(out PressInput input)
    {
        if (Active)
        {
            input = default;
            return false;
        }

        input = ProduceStart();
        return true;
    }

    protected override bool ProcessReleased(out PressInput input)
    {
        if (!Active)
        {
            input = default;
            return false;
        }

        input = ProduceStop();
        return true;
    }
}