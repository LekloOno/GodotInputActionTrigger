namespace GIAT.Interface;

using System.Diagnostics.CodeAnalysis;

public interface IBuffer<T> where T: IInput
{
    /// <summary>
    /// Tries to buffer the input.
    /// </summary>
    /// <param name="input">The input to buffer.</param>
    /// <returns>Whether the input was successfully buffered.</returns>
    bool Buffer(T input);
    /// <summary>
    /// Tries to consume the buffer, and return the underlying input.
    /// </summary>
    /// <param name="input">The (possibly invalid) consumed input.</param>
    /// <returns>false if there's no input to consume, meaning `input` is not valid, true otherwise.</returns>
    bool Consume([MaybeNullWhen(false)] out T input);
    /// <summary>
    /// Retrieves a buffered input without consuming it.
    /// </summary>
    /// <param name="input">The (possibly invalid) peaked input.</param>
    /// <returns>false if there's no input to peak, meaning `input` is not valid, true otherwise.</returns>
    bool Peak([MaybeNullWhen(false)] out T input);
    /// <summary>
    /// Clears the buffer.
    /// </summary>
    void Clear();
    /// <summary>
    /// Clears the provided input from the buffer.
    /// </summary>
    /// <param name="input"></param>
    void Clear(T input);
    /// <summary>
    /// Checks if the buffer is empty
    /// </summary>
    /// <returns>Whether this buffer is empty or not.</returns>
    bool IsEmpty();
}