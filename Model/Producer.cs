namespace GIAT.Model;

using System.Collections.Generic;
using GIAT.Interface;
using Godot;

public abstract class Producer<T> : IProducer<T>
{
    public List<ITrigger<T>> _triggers = [];
    public ulong LastInputStamp {get; protected set;}

    public bool Produce(out ISharedInput<T> input, InputEvent @event, InputContext context)
    {
        bool produced = ProduceInternal(out input, @event, context);
        if (produced)
            LastInputStamp = Time.GetTicksMsec();
        
        return produced;
    }

    public void Produce(InputEvent @event, InputContext context)
    {
        if (!Produce(out ISharedInput<T> input, @event, context))
            return;

        foreach (ITrigger<T> trigger in _triggers)
            trigger.Trigger(input);
    }

    protected abstract bool ProduceInternal(out ISharedInput<T> input, InputEvent @event, InputContext context);  
}