namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public class EmptyBuffer<T> : IBuffer<T>
{
    public bool Buffer(T input)
        => false;

    public bool Consume(out T input)
    {
        input = default;
        return false;   
    }

    public bool IsEmpty()
        => true;

    public bool Peak(out T input) =>
        Consume(out input);

    public void Clear() {}
    public void Pop() {}
}