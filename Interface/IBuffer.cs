namespace GIAT.Interface;

using System.Diagnostics.CodeAnalysis;

public interface IBuffer<T>
{
    /// <summary>
    /// Tries to buffer the input.
    /// </summary>
    /// <param name="input">The input to buffer.</param>
    /// <returns>Whether the input was successfully buffered.</returns>
    bool TryBuffer(ISharedInput<T> input);
    /// <summary>
    /// Tries to consume the next input from the buffer, and return the underlying input.
    /// </summary>
    /// <param name="input">The (possibly invalid) input of the consumed input.</param>
    /// <returns>false if there's no input to consume, meaning `signal` is not valid, true otherwise.</returns>
    bool TryConsume([MaybeNullWhen(false)] out ISharedInput<T> input);
    /// <summary>
    /// Retrieves a buffered input without consuming it. <br/>
    /// </summary>
    /// <param name="input">The (possibly invalid) peaked input.</param>
    /// <returns>false if there's no input to peak, meaning `input` is not valid, true otherwise.</returns>
    bool TryPeek([MaybeNullWhen(false)] out ISharedInput<T> input);
    /// <summary>
    /// Clears the buffer.
    /// </summary>
    void Clear();
    /// <summary>
    /// Clears the next input from the buffer without consuming it.
    /// <returns>Whether any input got cleared.</returns>
    /// </summary>
    bool Pop();
    /// <summary>
    /// Checks if the buffer is empty
    /// </summary>
    /// <returns>Whether this buffer is empty or not.</returns>
    bool IsEmpty();
}