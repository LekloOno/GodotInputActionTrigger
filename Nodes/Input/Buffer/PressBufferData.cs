namespace GIAT.Nodes.Input.Buffer;

using Godot;

using GIAT.Nodes.Input.Type;
using Godot.Collections;

[GlobalClass, Tool]
public partial class PressBufferData : Resource
{
    private PressInput _buffered = PressInput.Start | PressInput.Stop;

    /// <summary>
    /// When only start or stop is buffered, the other input (respectively stop and start)
    /// will unvalidate the previously buffered input.
    /// </summary>
    private bool _otherCancels = false;
    public bool Cancel
        => _otherCancels && _cancelAvailable;

    private bool _cancelAvailable
        => _buffered != (PressInput.Start | PressInput.Stop)
        && _buffered != 0;
}