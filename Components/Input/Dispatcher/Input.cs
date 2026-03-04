namespace GIAT.Components.Input.Dispatcher;

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

public class Input<T>(T newInput, InputState inputState) : IInput<T>
{
    private readonly T _input = newInput;
    public InputState Handled {get; private set;} = inputState;

    public bool Retrieve(out T input)
    {
        input = _input;
        return Handled;
    }

    public void SetHandled()
        => Handled.SetHandled();
}