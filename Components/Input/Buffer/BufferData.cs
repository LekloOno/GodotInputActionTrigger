namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public interface BufferData<T> where T : class, IInput
{
    public abstract IBuffer<T> Build();
}