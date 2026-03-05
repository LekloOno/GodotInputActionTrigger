namespace GIAT.Components.Input.Buffer;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GIAT.Components.Input.Dispatcher;

public class QueuedBuffer<T>(QueuedBufferData data) : BufferBase<T>
{
    protected readonly QueuedBufferData _data = data;
    private LinkedList<(Input<T>, ulong)> _buffered = new();
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

    public override bool IsEmpty()
    {
        Peak(out T _);
        return _buffered.Count != 0;
    }

    public override bool Peek([MaybeNullWhen(false)] out Input<T> input)
    {
        input = default;

        // Clean unvalid inputs and tries to find a valid one.
        while (_buffered.Count > 0)
        {
            (input, ulong stamp) = _buffered.Last.Value;

            if (!Expired(stamp))
                return true;

            Pop();
        }

        return false;
    }

    protected override List<Input<T>> GetInputs()
    {
        List<Input<T>> list = new(_buffered.Count);
        foreach (var (input, _) in _buffered)
            list.Add(input);

        return list;
    }

    protected override void ClearSpec()
        => _buffered.Clear();

    protected override bool ConsumeSpec([MaybeNullWhen(false)] out Input<T> input)    
    {
        if (!Peek(out input))
            return false;

        Pop();
        return true;
    }

    protected override bool ConsumeSpec(Input<T> input)
    {
        var node = _buffered.First;

        while (node != null)
        {
            if (EqualityComparer<T>.Default.Equals(node.Value.Item1, value))
            {
                linkedList.Remove(node);
                break;
            }
            node = node.Next;
        }
    }

    protected override bool BufferSpec(Input<T> input)
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

    protected override void PopInputSpec(Input<T> input)
    {
        throw new System.NotImplementedException();
    }

    protected override bool PopSpec([MaybeNullWhen(false)] out Input<T> input)
    {
        throw new System.NotImplementedException();
    }
}