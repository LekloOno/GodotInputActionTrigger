namespace GIAT.Components.Input.Dispatcher;

using Godot;

using GIAT.Interface;
using System.Collections.Generic;

public partial class InputDispatcher : Node
{
    public static InputDispatcher Instance;
    private static List<IInputStateHandler> _inputHandlers = new();

    public override void _Input(InputEvent @event)
    {
        InputState inputState = new();
        foreach (IInputStateHandler inputHandler in _inputHandlers)
            if (inputHandler.Handle(inputState, @event))
                return;
    }

    public override void _EnterTree()
    {
        Instance = this;
    }
}