namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;

public class SimpleBuffer<T>(SimpleBufferData data) : IBuffer<T>
{
    protected readonly SimpleBufferData _data = data;
    private T _buffered = default;
    public bool _containsInput = false;

    private ulong _timeStamp;

    private bool Expired()
        => _data.UseLifeTime 
        && PHX_Time.ScaledTicksMsec - _timeStamp > _data.LifeTime;

    public void Clear()
    {
        _buffered = default;
        _containsInput = false;
    }

    public void Pop() => Clear();

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