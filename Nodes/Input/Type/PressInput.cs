namespace GIAT.Nodes.Input.Type;

using GIAT.Interface;

public enum PressState
{
    Start,
    Stop,
}

public record PressInput(PressState state) : IInput;