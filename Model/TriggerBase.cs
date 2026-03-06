namespace GIAT.Model;

using GIAT.Interface;

public abstract class TriggerBase<T> : ITrigger<T>, IAction<ISharedInput<T>>
{
    private ClaimGuard _claimGuard;
    private ClaimMode _claimMode;

    protected bool TryClaim(ISharedInput<T> input, out T signal)
        => input.TryClaim(this, out signal, _claimGuard, _claimMode);

    protected void Unclaim(ISharedInput<T> input)
        => input.Unclaim(this, _claimMode);

    public bool Trigger(ISharedInput<T> input)
    {
        if (!TryClaim(input, out T signal))
            return false;

        if (TriggerSpec(input, signal))
            return true;

        Unclaim(input);
        return false;
    }

    /// <summary>
    /// Some specialized behavior to perform after the signal has been successfully claimed.
    /// </summary>
    /// <param name="input">The input from which the provided signal has been claimed.</param>
    /// <param name="signal">The claimed signal.</param>
    /// <returns>Whether the input should be unclaimed.</returns>
    protected abstract bool TriggerSpec(ISharedInput<T> input, T signal);

    public bool Do(ISharedInput<T> input)
        => ((ITrigger<T>)  this).Trigger(input);
}