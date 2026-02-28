namespace GIAT.Components.Input.Handler;

using System;
using Godot;

using GIAT.Interface;
using GIAT.Components.Trigger;
using GIAT.Components.Input.Buffer;

[Tool]
public abstract partial class InputHandler<T> : NodeTrigger<T>, IAction, IAction<T> where T: class, IInput
{
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


    private bool DoTrigger(T input)
    {
        foreach (IAction<T> action in _actions)
            if (action.Do(input))
                return true;
        
        return false;
    }

    public bool Do()
    {
        if (!_buffer.Consume(out T input))
            return false;

        return DoTrigger(input);        
    }

    public bool Do(T input)
    {
        LastInput = PHX_Time.ScaledTicksMsec;
        bool handled = _triggerHandler(input);

        if (handled)
            return true;

        _buffer.Buffer(input);
        return false;
    }

    protected override void CheckParentSpec()
    {
        if (GetParent() is ITrigger)
            _triggerHandler = (_) => false;
        else
            _triggerHandler = DoTrigger;
    }

    protected override void UnparentSpec()
    {
        _triggerHandler = DoTrigger;
    }
}