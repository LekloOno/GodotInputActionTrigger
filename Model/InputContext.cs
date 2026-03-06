namespace GIAT.Model;

using System.Collections.Generic;
using GIAT.Interface;
using Godot;

/// <summary>
/// Stores a shared input state.
/// This allows various sharers to mark the input as used globally.
/// </summary>
public class InputContext : IInputContext
{
    private HashSet<object> _claimers = new(ReferenceEqualityComparer.Instance);
    public ulong TimeStamp {get;} = Time.GetTicksMsec();

    public bool Claimed()
        => _claimers.Count == 0;

    public void Claim(object claimer, ClaimMode mode)
    {
        if (mode == ClaimMode.Exclusive)
            _claimers.Add(claimer);
    }

    public void Unclaim(object claimer, ClaimMode mode)
    {
        if (mode == ClaimMode.Exclusive)
            _claimers.Remove(claimer);
    }
}