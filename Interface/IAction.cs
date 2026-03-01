using GIAT.Nodes.Input.Type;

namespace GIAT.Interface;

public interface IAction<T>
{
    bool Do(T input);
}

public interface IAction : IAction<Unit>
{
    bool Do();
    bool IAction<Unit>.Do(Unit _) => Do();
}