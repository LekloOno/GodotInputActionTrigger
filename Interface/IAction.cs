using GIAT.Model;

namespace GIAT.Interface;

public interface IAction<T>
{
    /// <summary>
    /// Takes the signal and tries to perform an action with it.
    /// </summary>
    /// <param name="signal">The provided signal.</param>
    /// <returns>Whether the action was successfull or not.</returns>
    bool Do(T signal);
}

public interface IAction : IAction<Unit>
{
    /// <summary>
    /// Triggers an action.
    /// </summary>
    /// <returns>Whether the action was successfully or not.</returns>
    bool Do();
    bool IAction<Unit>.Do(Unit _)
        => Do();
}