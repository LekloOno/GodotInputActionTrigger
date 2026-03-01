namespace GIAT.Nodes.Input.Buffer;

using GIAT.Nodes.Input.Type;
using GIAT.Interface;
using System.Diagnostics.CodeAnalysis;

public class PressBuffer(IBuffer<PressInput> buffer, PressBufferData data): IBuffer<PressInput>
{
    public bool Buffer(PressInput input)
    {
        if (data.Buffered.HasFlag(input))
            return buffer.Buffer(input);
        
        if (data.Cancel)
            buffer.Pop();
            
        return false;
    }

    public void Clear()
        => buffer.Clear();

    public bool Consume([MaybeNullWhen(false)] out PressInput input)
        => buffer.Consume(out input);

    public bool IsEmpty()
        => buffer.IsEmpty();

    public bool Peak([MaybeNullWhen(false)] out PressInput input)
        => buffer.Peak(out input);

    public void Pop()
        => buffer.Pop();
}