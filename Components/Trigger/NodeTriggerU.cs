namespace GIAT.Components.Trigger;

using Godot;

using GIAT.Nodes.Input.Type;
using GIAT.Interface;
using GIAT.Components.Input.Dispatcher;

[GlobalClass, Tool]
public partial class NodeTriggerU : NodeTrigger<Unit>, IAction
{
    public bool Do()
        => Do(new Input<Unit>(Unit.Value, new()));

    protected override void CheckParentSpec(){}
    protected override void EnterTreeSpec(){}
    protected override void UnparentSpec(){}
}