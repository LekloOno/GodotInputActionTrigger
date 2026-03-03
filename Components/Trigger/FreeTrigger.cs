namespace GIAT.Components.Trigger;

using Godot;
using GIAT.Interface;


[GlobalClass]
public abstract partial class FreeTrigger : Node, ITransmitter
{
    public abstract bool Transmit();
}