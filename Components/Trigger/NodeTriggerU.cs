namespace GIAT.Components.Trigger;

using Godot;

using GIAT.Nodes.Input.Type;
using GIAT.Interface;

[GlobalClass, Tool]
public partial class NodeTriggerU : NodeTrigger<Unit>, IAction
{
    public bool Do()
        => Do(Unit.Value);

    protected override void CheckParentSpec(){}
    protected override void EnterTreeSpec(){}
    protected override void UnparentSpec(){}
}