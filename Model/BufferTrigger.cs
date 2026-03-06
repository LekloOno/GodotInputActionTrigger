namespace GIAT.Model;

using System.Diagnostics.CodeAnalysis;
using GIAT.Interface;

public abstract class BufferTrigger<T> : TriggerBase<T>, IAction
{
    /// <summary>
    /// Whether the buffer trigger should trigger its action on
    /// shared input reception, or only on external trigger.
    /// </summary>
    protected bool _selfTrigger = true;
    protected IBuffer<T> _buffer;
    protected IAction<T> _action;

    protected override bool TriggerSpec(ISharedInput<T> input, T signal)
    {
        if (_selfTrigger && _action.Do(signal))
            return true;
        
        _buffer.TryBuffer(input);
        return false;
    }

    public bool Do()
    {
        if (!_buffer.TryPeek(out ISharedInput<T> input))
            return false;

        if (!TryClaim(input, out T signal))
            return false;

        if (!_action.Do(signal))
        {
            Unclaim(input);
            return false;
        }

        _buffer.Pop();
        return true;
    }

    public void Clear()
        => _buffer.Clear();

    public bool IsEmpty()
        => _buffer.IsEmpty();
    public bool Pop()
        => _buffer.Pop();
    public bool TryBuffer(ISharedInput<T> input)
        => _buffer.TryBuffer(input);
    public bool TryConsume([MaybeNullWhen(false)] out ISharedInput<T> input)
        => _buffer.TryConsume(out input);
    public bool TryPeek([MaybeNullWhen(false)] out ISharedInput<T> input)
        => _buffer.TryPeek(out input);
}