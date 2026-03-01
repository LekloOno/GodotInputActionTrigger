namespace GIAT.Interface;

using Godot;

public interface IProducer<T>
{
    /// <summary>
    /// Tries to produce an input from the provided input event.
    /// </summary>
    /// <param name="event">The input event to process.</param>
    /// <param name="input">The (possibly unvalid) produced input.</param>
    /// <returns>Whether the produced input is valid.</returns>
    bool Produce(InputEvent @event, out T input);
    /// <summary>
    /// Called to simulate an externally produced input as if it was internally produced. <br/>
    /// <br/>
    /// If `input` can be a valid input in the current state of the producer,
    /// it should call the procedures usually taken when `Produce` does produce such an input.
    /// </summary>
    /// <param name="input">The virtual input.</param>
    /// <returns>Wether the provided input could be a valid produced input.</returns>
    bool ProduceExternal(T input);
}