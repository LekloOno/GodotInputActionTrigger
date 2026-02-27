namespace GIAT.Nodes.Action;

using GIAT.Interface;
using Godot;

[GlobalClass]
public abstract partial class PressAction : Node, IAction<PressAction>
{
    public abstract bool Do(PressAction input);
}