namespace GIAT.Components.Input.Handler;

using Godot;

using GIAT.Interface;
using GIAT.Components.Trigger;
using GIAT.Components.Input.Buffer;
using System.Collections.Generic;
using System;
using GIAT.Components.Input.Dispatcher;

[Tool]
public abstract partial class InputHandler<T> : NodeTrigger<T>, IAction, IInputStateHandler
{
    private List<IInputStateHandler> _inputHandlers = [];

    protected string _actionName = "";
    /// <summary>
    /// Makes so the input handler consumes its own inputs.
    /// </summary>
    private bool _consumeSelf = true;
    /// <summary>
    /// Makes so only successfull call to Do() consume used buffer input.
    /// </summary>
    [Export]
    private bool _consumeSuccessOnly = true;
    /// <summary>
    /// Makes so the input handler produces its own inputs
    /// </summary>
    private bool _produceSelf = true;

    private bool _enabled = true;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            SetProcessUnhandledInput();   
        }
    }

    private void SetProcessUnhandledInput()
        => SetProcessUnhandledInput(_produceSelf && _enabled && _producer != null);

    private bool IsTrigger => _consumeSelf && _rootTrigger;
    public ulong LastInputStamp => _producer.LastInputStamp;
    private bool _rootTrigger = false;

    protected IProducer<T> _producer;
    public BufferData _bufferData;
    private IBuffer<T> _buffer = new EmptyBuffer<T>();

    public override void _Ready()
        => SetProcessUnhandledInput();

    protected override void EnterTreeSpec()
        => SetProcessUnhandledInput();


    public bool Handle(InputState inputState, InputEvent @event)
    {
        bool handled = HandleInternal(inputState, @event);
        if (handled)
            inputState.SetHandled();
        
        return handled;
    }

    public bool HandleInternal(InputState inputState, InputEvent @event)
    {
        if (!@event.IsAction(_actionName))
            return Transmit(inputState, @event);

        if(!_produceSelf)
            return Transmit(inputState, @event);

        if (_producer == null)
            return Transmit(inputState, @event);

        if(!_producer.Produce(inputState, @event, out IInput<T> input))
            return Transmit(inputState, @event);

        if(Transmit(inputState, @event, input))
            return true;

        _buffer.Buffer(input);
        return false;
    }

    private bool Transmit(InputState inputState, InputEvent @event, IInput<T> input)
    {
        if (_consumeSelf && input.Retrieve(out T signal))
        {
            if (HandlerTransmit(inputState, @event, signal))
                return true;
        }
        else
        {
            if (Transmit(inputState, @event))
                return true;
        }

        return false;
    }

    private bool HandlerTransmit(InputState inputState, InputEvent @event, T signal)
    {
        foreach (Node node in GetChildren())
        {
            if (node is IInputStateHandler inputHandler)
            {
                if (inputHandler.Handle(inputState, @event))
                    return true;
            }
            else if (node is IAction<T> action)
            {
                if (action.Do(signal))
                {
                    
                    return true;
                }
            }
        }
        
        return false;
    }

    public bool Transmit(InputState state, InputEvent @event)
    {
        foreach (IInputStateHandler inputHandler in _inputHandlers)
            if (inputHandler.Handle(state, @event))
                return true;

        return false;
    }

    public override bool Do(IInput<T> input)
    {
        if (Transmit(input))
            return true;

        // Problem spotted :
        // Buffer might need to be called from the exterior.
        // If an action fails, it will instantly try to buffer input,
        // although one of its siblings actions might have succeeded.
        // We should buffer if none did, but then where ?
        // In the parent ? then we moved the problem up

        // Gotta figure this out !

        // Maybe never buffer the input i get sent, only the one i produce ?
        if (_consumeSuccessOnly)
            return _buffer.Buffer(input);

        return true;
    }

    public bool Do()
    {
        if (!_buffer.Consume(out IInput<T> input))
            return false;

        return Do(input);
    }
}