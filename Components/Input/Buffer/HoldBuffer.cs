namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

/// <summary>
/// A hold buffer is similar to SimpleBuffer, but do not free the buffered input on consumption.  
/// You can use this type of buffer to change from a "on press" trigger behavior to "on hold".
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="innerBuffer"></param>
public class HoldBuffer<T>(SimpleBuffer<T> innerBuffer) : IBuffer<T>
{
    public void Clear()
        => innerBuffer.Clear();

    public void Pop() {}

    public virtual bool Buffer(T input)
        => innerBuffer.Buffer(input);

    public bool Consume(out T input)
        => innerBuffer.Peak(out input);

    public bool Peak(out T input)
        => innerBuffer.Peak(out input);

    public bool IsEmpty()
        => innerBuffer.IsEmpty();
}