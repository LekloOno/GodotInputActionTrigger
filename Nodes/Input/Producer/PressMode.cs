namespace GIAT.Nodes.Input.Producer;

public enum PressMode { Hold, Tap }

public static class PressModeExtensions
{
    public static PressInputProducer Producer(this PressMode mode)
    {
        return mode switch
        {
            PressMode.Tap => new TapProducer(),
            PressMode.Hold => new HoldProducer(),
            _ => new TapProducer(),
        };
    }
}