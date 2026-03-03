namespace GIAT.Interface;

using GIAT.Nodes.Input.Type;

public interface ITransmitter<T>
{
    bool Transmit(T input);    
}

public interface ITransmitter: ITransmitter<Unit>
{
    bool Transmit();
    bool ITransmitter<Unit>.Transmit(Unit _) => Transmit();    
}