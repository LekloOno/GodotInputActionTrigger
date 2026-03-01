namespace GIAT.Components.Trigger;

using System.Collections.Generic;
using Godot;

using GIAT.Interface;

[Tool]
public abstract partial class NodeTrigger<T> : FreeTrigger, IAction<T>
{
    protected List<IAction<T>> _actions = [];

    public bool Do(T input)
    {
        foreach (IAction<T> action in _actions)
            if (action.Do(input))
                return true;

        return false;
    }

    private void CheckParent()
    {
        CheckParentSpec();

        if (_actions.Count != 0)
            return;

        if (GetParent() is not IAction<T> action)
            return;

        // Avoid infinite recursion with Trigger actions
        if (action is NodeTrigger<T>)
            return;

        _actions = [action];
    }

    /// <summary>
    /// Some extended behavior on parent checking.
    /// </summary>
    /// <returns>Whether the extension successfully completed the check, thus not requiring further operations from the base.</returns>
    protected abstract void CheckParentSpec();

    protected virtual void CheckChildren()
    {
        List<IAction<T>> actions = [];
        foreach(Node node in GetChildren())
            if (node is IAction<T> action)
                actions.Add(action);
        
        // In the case where the trigger has a parent action,
        // and a new unrelated node appears in children list.
        // Prevent _actions from being emptied.
        if (actions.Count != 0)
            _actions = actions;
        else if (_actions.Count != 0 && _actions[0] != GetParent())
            _actions = actions;
    }

    private void Unparent()
    {
        UnparentSpec();
         
        if (_actions.Count == 0)
            return;
        
        if (_actions[0] == GetParent())
            _actions = [];
    }

    protected abstract void UnparentSpec();

    public override sealed void _EnterTree()
    {
        CheckChildren();
        CheckParent();
        EnterTreeSpec();
    }

    protected abstract void EnterTreeSpec();

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