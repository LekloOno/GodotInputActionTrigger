namespace GIAT.Components.Input.Buffer;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GIAT.Components.Input.Dispatcher;
using Godot;

public class SimpleBuffer<T>(SimpleBufferData data) : BufferBase<T>
{
    protected readonly SimpleBufferData _data = data;
    private Input<T> _buffered = default;
    public bool _containsInput = false;

    private ulong _timeStamp;

    private bool Expired()
        => _data.UseLifeTime 
        && PHX_Time.ScaledTicksMsec - _timeStamp > _data.LifeTime;

    protected override void ClearSpec()
        => _containsInput = false;

    public override bool Peek(out Input<T> input)
    {
        input = _buffered;
        if (Expired())
            Pop();

        return _containsInput;
    }

    public override bool IsEmpty() =>
        Expired() || !_containsInput;

    protected override bool ConsumeSpec(out Input<T> input)
    {
        if (!Peek(out input))
            return false;
        
        _containsInput = false;
        return true;
    }

    protected override bool ConsumeSpec(Input<T> input)
    {
        if (!Peek(out _))
            return false;

        if (_buffered != input)
            return false;
        
        _containsInput = false;
        return true;
    }

    protected override bool BufferSpec(Input<T> input)
    {
        if (input is null)
            return false;

        _timeStamp = Time.GetTicksMsec();
        _buffered = input; 
        _containsInput = true;
        return true;
    }

    protected override void PopInputSpec(Input<T> input)
    {
        if (_buffered == input)
            _containsInput = false;
    }

    protected override bool PopSpec([MaybeNullWhen(false)] out Input<T> input)
    {
        input = _buffered;
        if (!_containsInput)
            return false;
        
        _containsInput = false;
        return true;
    }

    protected override List<Input<T>> GetInputs()
        => [_buffered];
}