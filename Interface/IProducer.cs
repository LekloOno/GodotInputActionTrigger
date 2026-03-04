namespace GIAT.Interface;

using GIAT.Components.Input.Dispatcher;
using Godot;

public interface IProducer<U, T>
{
    public ulong LastInputStamp {get;}
    /// <summary>
    /// Tries to produce an input from the provided input event.
    /// </summary>
    /// <param name="event">The input event to process.</param>
    /// <param name="input">The (possibly unvalid) produced input.</param>
    /// <returns>Whether the produced input is valid.</returns>
    bool Produce(InputState state, U @event, out IInput<T> input);
}

public interface IProducer<T> : IProducer<InputEvent, T>;