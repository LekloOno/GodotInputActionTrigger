using GIAT.Components.Input.Dispatcher;
using Godot;

namespace GIAT.Interface;

public interface IInputStateHandler
{
    bool Handle(InputState inputState, InputEvent @event);
}