namespace GIAT.Components.Input.Dispatcher;

using System;
using GIAT.Interface;

/// <summary>
/// Stores a shared input state.
/// This allows various sharers to mark the input as used globally.
/// </summary>
public class InputState()
{
    public bool Handled = false;

    public void SetHandled()
        => Handled = true;

    public static implicit operator bool(InputState inputState)
        => inputState.Handled;
}

public class Input<T>(T newSignal, InputState inputState) : IInput<T>
{
    public T Signal {get;} = newSignal;
    public InputState Handled {get; private set;} = inputState;
    public event Action<Input<T>> OnHandle;

    public void SetHandled()
    {
        Handled.SetHandled();
        OnHandle?.Invoke(this);
    }
}