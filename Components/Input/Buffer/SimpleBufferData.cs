namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public interface SimpleBufferData<T> : BufferData<T> where T : IInput
{
    bool UseLifeTime {get;}
    ulong LifeTime {get;}
}