namespace GIAT.Components.Input.Buffer;

using System.Diagnostics.CodeAnalysis;
using GIAT.Components.Input.Dispatcher;
using GIAT.Interface;

/// <summary>
/// A hold buffer is similar to SimpleBuffer, but do not free the buffered input on consumption.  
/// You can use this type of buffer to change from a "on press" trigger behavior to "on hold".
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="innerBuffer"></param>
public class HoldBuffer<T>(SimpleBuffer<T> innerBuffer) : BufferBase<T>
{

    public void Pop() {}

    public virtual bool Buffer(IInput<T> input)
        => innerBuffer.Buffer(input);

    public override void Clear()
        => innerBuffer.Clear();

    public override bool IsEmpty()
        => innerBuffer.IsEmpty();

    public override bool Peek([MaybeNullWhen(false)] out Input<T> input)
        => innerBuffer.Peek(out input);

    protected override bool ConsumeSpec([MaybeNullWhen(false)] out Input<T> input)
    {
        throw new System.NotImplementedException();
    }

    protected override bool ConsumeSpec(Input<T> input)
    {
        throw new System.NotImplementedException();
    }

    protected override bool BufferSpec(Input<T> input)
    {
        throw new System.NotImplementedException();
    }

    protected override void PopInputSpec(Input<T> input)
    {
        throw new System.NotImplementedException();
    }

    protected override bool PopSpec([MaybeNullWhen(false)] out Input<T> input)
    {
        throw new System.NotImplementedException();
    }
}