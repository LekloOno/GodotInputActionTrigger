namespace GIAT.Components.Input.Producer;

using Godot;

using GIAT.Interface;

public abstract class Producer<U, T> : IProducer<U, T>
{
    public ulong LastInputStamp {get; protected set;}

    public bool Produce(U @event, out T input)
    {
        bool produced = ProduceInternal(@event, out input);
        
        if (produced)
            LastInputStamp = Time.GetTicksMsec();

        return produced;
    }

    public abstract bool ProduceInternal(U @event, out T input);   
}