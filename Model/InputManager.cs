namespace GIAT.Model;

using System.Collections.Generic;
using GIAT.Interface;
using Godot;

public partial class InputManager : Node
{
    private static List<IProducer> _producers = [];
    public override void _UnhandledInput(InputEvent @event)
    {
        InputContext context = new();
        
        foreach (IProducer producer in _producers)
            producer.Produce(@event, context);
    }
}