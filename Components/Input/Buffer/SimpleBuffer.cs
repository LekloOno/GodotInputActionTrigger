namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public class SimpleBuffer<T, D>(D data) : IBuffer<T>
    where T : IInput
    where D: SimpleBufferData<T>
{
    protected readonly D _data = data;
    private ulong _lifeTime => _data.LifeTime;
    private T _buffered = default;
    public bool _containsInput = false;

    private ulong _timeStamp;

    private bool Expired()
        => _lifeTime != 0 
        && PHX_Time.ScaledTicksMsec - _timeStamp > _lifeTime;

    public void Clear()
    {
        _buffered = default;
        _containsInput = false;
    }

    public void Clear(T _) => Clear();

    public virtual bool Buffer(T input)
    {
        if (input is null)
            return false;

        _timeStamp = PHX_Time.ScaledTicksMsec;
        _buffered = input;
        _containsInput = true;
        return true;
    }

    public bool Consume(out T input)
    {
        if (!Peak(out input))
            return false;

        Clear();
        return true;
    }

    public bool Peak(out T input)
    {
        input = _buffered;
        if (Expired())
            Clear();

        return _containsInput;
    }

    public bool IsEmpty() =>
        Expired() || !_containsInput;
}