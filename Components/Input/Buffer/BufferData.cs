namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public interface BufferData<T> where T : IInput
{
    public abstract IBuffer<T> Build();
}