namespace GIAT.Interface;

public interface ITrigger<T>
{
    /// <summary>
    /// Takes the shared input and tries to pass its signal to an action.
    /// </summary>
    /// <param name="input">The provided input.</param>
    /// <returns>Whether the trigger successfully triggered the action.</returns>
    bool Trigger(ISharedInput<T> input);
}