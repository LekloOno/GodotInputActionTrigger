namespace GIAT.Nodes.Input.Buffer;

using GIAT.Nodes.Input.Type;
using GIAT.Components.Input.Buffer;

public class SimplePressBuffer(SimplePressBufferData data): SimpleBuffer<PressInput, SimplePressBufferData>(data)
{
    public override bool Buffer(PressInput input)
    {
        if (!_data.Buffered.HasFlag(input.state))
            return false;
        
        return base.Buffer(input);
    }
}