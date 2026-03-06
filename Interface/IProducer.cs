namespace GIAT.Interface;

using GIAT.Model;
using Godot;


public interface IProducer
{
    void Produce(InputEvent @event, InputContext context);
}

public interface IProducer<T> : IProducer
{
    /// <summary>
    /// Tries to produce a shared input of T from the provided input event and associated context. 
    /// </summary>
    /// <param name="input">The (possibly invalid) produced shared input.</param>
    /// <param name="event">The input event to produce from.</param>
    /// <param name="context">The context to tie the shared input to.</param>
    /// <returns>Whether the produced shared input is valid.</returns>
    bool Produce(out ISharedInput<T> input, InputEvent @event, InputContext context);
}