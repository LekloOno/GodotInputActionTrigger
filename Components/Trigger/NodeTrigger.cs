namespace GIAT.Components.Trigger;

using System.Collections.Generic;
using Godot;

using GIAT.Interface;

[Tool]
public abstract partial class NodeTrigger<T> : FreeTrigger
{
    protected List<IAction<T>> _actions = [];

    private void CheckParent()
    {
        CheckParentSpec();

        if (_actions.Count != 0)
            return;

        if (GetParent() is not IAction<T> action)
            return;

        if (action is NodeTrigger<T>)
            return;

        _actions = [action];
    }

    protected abstract void CheckParentSpec();

    private void CheckChildren()
    {
        CheckChildrenSpec();
        List<IAction<T>> actions = [];
        foreach(Node node in GetChildren())
            if (node is IAction<T> action)
                actions.Add(action);
        
        // In the case where the trigger has a parent action,
        // and a new unrelated node appears in children list.
        // Prevent _actions from being emptied.
        if (actions.Count != 0)
            _actions = actions;
    }

    protected abstract void CheckChildrenSpec();

    private void Unparent()
    {
        UnparentSpec();
        _actions = [];
    }

    protected abstract void UnparentSpec();

    public override sealed void _EnterTree()
    {
        CheckChildren();
        CheckParent();
    }

    public override sealed void _Notification(int what)
    {
        switch ((long) what)
        {
            case NotificationParented:
                CheckParent();
                break;
            case NotificationUnparented:
                Unparent();
                break;
            case NotificationChildOrderChanged:
                CheckChildren();
                break;
            default:
                return;
        }

        UpdateConfigurationWarnings();
        NotifyPropertyListChanged();
    }
}