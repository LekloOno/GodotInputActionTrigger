namespace GIAT.Interface;

/// <summary>
/// Stores a sharable input, that can be unvalidated for the other sharers.
/// </summary>
/// <typeparam name="T">The stored signal type.</typeparam>
public interface ISharedInput<T>
{
    /// <summary>
    /// /// <summary>
    /// Claims the input and retrieves the underlying signal. 
    /// </summary>
    /// <param name="claimer">The claimer.</param>
    /// <param name="signal">The (possibly invalid) underlying signal</param>
    /// <param name="guard">The rules applied to retrieve the signal.</param>
    /// <param name="mode">The rules applied to claim the input.</param>
    /// <returns>Whether the retrieved signal is valid or not.</returns>
    bool TryClaim(
        object claimer,
        out T signal,
        ClaimGuard guard = ClaimGuard.Guarded,
        ClaimMode mode = ClaimMode.Exclusive);
    
    /// <summary>
    /// Unclaims the input.
    /// </summary>
    /// <param name="claimer">The claimer.</param>
    /// <param name="mode">The rules applied to unclaim the input.</param>
    void Unclaim(object claimer, ClaimMode mode = ClaimMode.Exclusive);
}

/// <summary>
/// Specify the rule applied to retrieve a shared input's signal.
/// </summary>
public enum ClaimGuard
{
    /// <summary>
    /// Only retrieve the signal if the input has not been claimed yet.
    /// </summary>
    Guarded,
    /// <summary>
    /// Always retrieve the signal.
    /// </summary>
    Free,
}