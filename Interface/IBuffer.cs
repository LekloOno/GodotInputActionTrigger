namespace GIAT.Interface;

using System.Diagnostics.CodeAnalysis;
using GIAT.Components.Input.Dispatcher;

public interface IBuffer<T>
{
    /// <summary>
    /// Tries to buffer the input.
    /// </summary>
    /// <param name="input">The input to buffer.</param>
    /// <returns>Whether the input was successfully buffered.</returns>
    bool Buffer(Input<T> input);
    /// <summary>
    /// Tries to consume the buffer, and return the underlying input.
    /// </summary>
    /// <param name="signal">The (possibly invalid) signal of the consumed input.</param>
    /// <returns>false if there's no input to consume, meaning `signal` is not valid, true otherwise.</returns>
    bool Consume([MaybeNullWhen(false)] out T signal);
    /// <summary>
    /// Asks the buffer to consume the provided input.
    /// </summary>
    /// <param name="input">The input to consume.</param>
    /// <returns>false if such input can't be consumed by the buffer, typically if it does not store it.</returns>
    bool Consume(Input<T> input);
    /// <summary>
    /// Retrieves a buffered input without consuming it. <br/>
    /// To consume it safely, the user should then call `Consume(input)`. 
    /// </summary>
    /// <param name="input">The (possibly invalid) peaked input.</param>
    /// <returns>false if there's no input to peak, meaning `input` is not valid, true otherwise.</returns>
    bool Peek([MaybeNullWhen(false)] out Input<T> input);
    /// <summary>
    /// Clears the buffer.
    /// </summary>
    void Clear();
    /// <summary>
    /// Clears one input from the buffer.
    /// </summary>
    void Pop();
    /// <summary>
    /// Checks if the buffer is empty
    /// </summary>
    /// <returns>Whether this buffer is empty or not.</returns>
    bool IsEmpty();
}