namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public interface SimpleBufferData<T> : BufferData<T> where T : IInput
{
    ulong LifeTime {get;}
}