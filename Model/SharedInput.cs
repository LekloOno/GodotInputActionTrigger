namespace GIAT.Model;

using GIAT.Interface;

/// <summary>
/// Ties a signal to an input context that can be unvalidated
/// for other sharers.
/// </summary>
/// <typeparam name="T">The stored signal type.</typeparam>
/// <param name="_signal">The signal to tie the provided context.</param>
/// <param name="context">The input context of this shared input.</param>
public class SharedInput<T>(T _signal, InputContext context) : ISharedInput<T>
{
    public bool TryClaim(object claimer, out T signal, ClaimGuard guard = ClaimGuard.Guarded, ClaimMode mode = ClaimMode.Exclusive)
    {
        signal = _signal;
        if (guard == ClaimGuard.Guarded && context.Claimed())
            return false;

        context.Claim(claimer, mode);
        return true;
    }

    public void Unclaim(object claimer, ClaimMode mode = ClaimMode.Exclusive)
        => context.Unclaim(claimer, mode);
}