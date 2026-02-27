namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public class SimpleBuffer<T, D>(D data) : IBuffer<T>
    where T : class, IInput
    where D: SimpleBufferData<T>
{
    protected readonly D _data = data;
    private ulong _lifeTime => _data.LifeTime;
    private T _buffered;

    private ulong _bufferTime;

    private bool Expired()
        => _lifeTime != 0 
        && PHX_Time.ScaledTicksMsec - _bufferTime > _lifeTime;

    private T Buffered()
    {
        if (Expired())
            _buffered = null;

        return _buffered;
    }

    public virtual bool Buffer(T input)
    {
        _bufferTime = PHX_Time.ScaledTicksMsec;
        _buffered = input;
        return true;
    }

    public bool Consume(out T input)
    {
        input = Buffered();
        _buffered = null;
        return input == null;
    }

    public bool IsEmpty() =>
        Expired() || _buffered == null;

    public bool Peak(out T input)
    {
        input = Buffered();
        return input == null;
    }
}