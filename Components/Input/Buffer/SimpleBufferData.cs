namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public interface SimpleBufferData<T> : BufferData<T> where T : class, IInput
{
    ulong LifeTime {get;}
}