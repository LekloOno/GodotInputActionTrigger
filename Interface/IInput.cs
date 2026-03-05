using System;

namespace GIAT.Interface;

/// <summary>
/// Stores a sharable input, that can be unvalidated for the other sharers.
/// </summary>
/// <typeparam name="T">The stored input type.</typeparam>
public interface IInput<T>
{
    T Signal {get;}
    /// <summary>
    /// Marks the input as handled, making it unvalid for further use.
    /// </summary>
    void SetHandled();
}