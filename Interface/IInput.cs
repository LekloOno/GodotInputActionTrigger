namespace GIAT.Interface;

/// <summary>
/// Stores a sharable input, that can be unvalidated for the other sharers.
/// </summary>
/// <typeparam name="T">The stored input type.</typeparam>
public interface IInput<T>
{
    /// <summary>
    /// Tries to retrieve the T input.
    /// </summary>
    /// <param name="input">The (possibly unvalid) retrieved input</param>
    /// <returns>Whether the retrieved input is valid.</returns>
    bool Retrieve(out T input);
    /// <summary>
    /// Marks the input as handled, making it unvalid for further use.
    /// </summary>
    void SetHandled();
}