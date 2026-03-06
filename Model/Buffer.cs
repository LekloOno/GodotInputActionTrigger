namespace GIAT.Model;

using System.Diagnostics.CodeAnalysis;
using GIAT.Interface;

public abstract class Buffer<T> : IBuffer<T>, IAction<ISharedInput<T>>
{
    public bool Do(ISharedInput<T> input)
        => TryBuffer(input);

    public abstract void Clear();
    public abstract bool IsEmpty();
    public abstract bool Pop();
    public abstract bool TryBuffer(ISharedInput<T> input);
    public abstract bool TryConsume([MaybeNullWhen(false)] out ISharedInput<T> input);
    public abstract bool TryPeek([MaybeNullWhen(false)] out ISharedInput<T> input);
}