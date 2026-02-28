namespace GIAT.Components.Input.Handler;

using System;
using Godot;

using GIAT.Interface;
using GIAT.Components.Trigger;
using GIAT.Components.Input.Buffer;

[Tool]
public abstract partial class InputHandler<T> : NodeTrigger<T>, IAction, IAction<T> where T: IInput
{
    /// <summary>
    /// Makes so only successfull call to Do() consume used buffer input.
    /// </summary>
    [Export] private bool _consumeSuccessOnly = true;

    public ulong LastInput {get; private set;}
    private Func<T, bool> _triggerHandler;
    private IBuffer<T> _buffer;
    
    public BufferData<T> _bufferData;
    public BufferData<T> BufferData
    {
        get => _bufferData;
        protected set
        {
            if (_bufferData == value)
                return;

            _bufferData = value;
            
            if (value == null)
                _buffer = new EmptyBuffer<T>();
            else
                _buffer = value.Build();
        }
    }

    private bool TriggerActions(T input)
    {
        foreach (IAction<T> action in _actions)
            if (action.Do(input))
                return true;
        
        return false;
    }

    private bool SuccessDo()
    {
        if (!_buffer.Peak(out T input))
            return false;

        bool handled = TriggerActions(input);

        if (handled)
            _buffer.Clear(input);

        return handled;
    }

    private bool SimpleDo()
    {
        if (!_buffer.Consume(out T input))
            return false;

        return TriggerActions(input);
    }

    public bool Do()
    {
        if (_consumeSuccessOnly)
            return SuccessDo();
        return SimpleDo();
    }

    public bool Do(T input)
    {
        DoSpec(input);
        LastInput = PHX_Time.ScaledTicksMsec;
        bool handled = _triggerHandler(input);

        if (handled)
            return true;

        _buffer.Buffer(input);
        return false;
    }

    public abstract void DoSpec(T input);

    protected override void CheckParentSpec()
    {
        if (GetParent() is ITrigger)
            _triggerHandler = (_) => false;
        else
            _triggerHandler = TriggerActions;
    }

    protected override void UnparentSpec()
    {
        _triggerHandler = TriggerActions;
    }
}