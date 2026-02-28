namespace GIAT.Nodes.Action;

using GIAT.Interface;
using GIAT.Nodes.Input.Type;
using Godot;

[GlobalClass]
public abstract partial class PressAction : Node, IAction<PressInput>
{
    public event System.Action OnStart;
    public event System.Action OnStop;
    public bool Do(PressInput input)
    {
        return input switch
        {
            PressInput.Start => DoStart(),
            PressInput.Stop => DoStop(),
            _ => false,
        };
    }

    private bool DoStart()
    {
        bool handled = Start();
        OnStart?.Invoke();
        return handled;
    }

    private bool DoStop()
    {
        bool handled = Stop();
        OnStop?.Invoke();
        return handled;
    }

    public abstract bool Start();
    public abstract bool Stop();
}