namespace GIAT.Interface;

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
    /// <param name="input">The (possibly null) consumed input.</param>
    /// <returns>false if there's no input to consume, meaning `input` is null, true otherwise.</returns>
    bool Consume(out T input);
    /// <summary>
    /// Retrieves a buffered input without consuming it.
    /// </summary>
    /// <param name="input">The (possibly null) peaked input.</param>
    /// <returns>false if there's no input to peak, meaning `input` is null, true otherwise.</returns>
    bool Peak(out T input);
    /// <summary>
    /// Checks if the buffer is empty
    /// </summary>
    /// <returns>Whether this buffer is empty or not.</returns>
    bool IsEmpty();
}