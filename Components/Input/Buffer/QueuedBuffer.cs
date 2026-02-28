namespace GIAT.Components.Input.Buffer;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GIAT.Interface;

public class QueuedBuffer<T>(QueuedBufferData data) : IBuffer<T>
{
    protected readonly QueuedBufferData _data = data;
    private Queue<(T, ulong)> _buffered = new();
    public bool _containsInput = false;

    private bool Expired(ulong stamp)
        => _data.UseLifeTime 
        && PHX_Time.ScaledTicksMsec - stamp > _data.LifeTime;

    public virtual bool Buffer(T input)
    {
        if (input is null)
            return false;

        if (_buffered.Count >= _data.Size)
        {
            Peak(out _);
            if (_buffered.Count >= _data.Size)
                return false;
        }

        _buffered.Enqueue((input, PHX_Time.ScaledTicksMsec));
        return true;
    }

    public void Clear() =>
        _buffered.Clear();

    public void Pop() =>
        _buffered.Dequeue();

    public bool Consume([MaybeNullWhen(false)] out T input)
    {
        if (!Peak(out input))
            return false;

        Pop();
        return true;
    }

    public bool IsEmpty()
    {
        Peak(out T _);
        return _buffered.Count != 0;
    }

    public bool Peak([MaybeNullWhen(false)] out T input)
    {
        input = default;

        // Clean unvalid inputs and tries to find a valid one.
        while (_buffered.Count > 0)
        {
            (input, ulong stamp) = _buffered.Peek();
            if (!Expired(stamp))
                return true;

            Pop();
        }

        return false;
    }
}