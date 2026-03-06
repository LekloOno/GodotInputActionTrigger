namespace GIAT.Interface;

public interface IInputContext
{
    /// <summary>
    /// Returns whether the context is already claimed.
    /// </summary>
    /// <returns>Whether the context is already claimed.</returns>
    bool Claimed();
    ulong TimeStamp {get;}
    /// <summary>
    /// Claims the context.
    /// </summary>
    /// <param name="claimer">The claimer.</param>
    /// <param name="mode">The rules applied to claim the context.</param>
    void Claim(object claimer, ClaimMode mode);
    /// <summary>
    /// Unclaims the context.
    /// </summary>
    /// <param name="claimer">The claimer.</param>
    /// <param name="mode">The rules applied to unclaim the context.</param>
    void Unclaim(object claimer, ClaimMode mode);
}

/// <summary>
/// Specify the rule applied to the input claim.
/// </summary>
public enum ClaimMode
{
    /// <summary>
    /// Claim the input.
    /// </summary>
    Exclusive,
    /// <summary>
    /// Do not claim the input.
    /// </summary>
    Silent,
}