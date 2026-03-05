namespace GIAT.Components.Input.Buffer;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GIAT.Components.Input.Dispatcher;
using GIAT.Interface;

public abstract class BufferBase<T> : IBuffer<T>
{
    /// <summary>
    /// If false, it will not release the shared inputs that are consumed by other buffers.
    /// </summary>
    public bool Release {get; set;} = true;
    /// <summary>
    /// If false, it will not set the shared inputs as handled when consumed.
    /// </summary>
    public bool Handle {get; set;} = true;

    public abstract bool IsEmpty();
    public abstract bool Peek([MaybeNullWhen(false)] out Input<T> input);
    protected abstract List<Input<T>> GetInputs();
    protected abstract void ClearSpec();
    protected abstract bool ConsumeSpec([MaybeNullWhen(false)] out Input<T> input);
    protected abstract bool ConsumeSpec(Input<T> input);
    protected abstract bool BufferSpec(Input<T> input); 
    protected abstract void PopInputSpec(Input<T> input);
    protected abstract bool PopSpec([MaybeNullWhen(false)] out Input<T> input);

    public void Clear()
    {
        foreach (Input<T> input in GetInputs())
            input.OnHandle -= PopInput;

        ClearSpec();
    }

    public bool Consume([MaybeNullWhen(false)] out T signal)
    {
        if (!ConsumeSpec(out Input<T> input))
        {
            signal = default;
            return false;
        }

        signal = input.Signal;
        HandleInputState(input);

        return true;
    }

    public bool Consume(Input<T> input)
    {
        if (!ConsumeSpec(input))
            return false;
        
        HandleInputState(input);
        return true;
    }


    public void Pop()
    {
        if (PopSpec(out Input<T> input))
            input.OnHandle -= PopInput;
    }

    public bool Buffer(Input<T> input)
    {
        bool buffered = BufferSpec(input);
        if (buffered && Release)
            input.OnHandle += PopInput;

        return buffered;
    }

    private void PopInput(Input<T> input)
    {
        input.OnHandle -= PopInput;
        PopInputSpec(input);
    }

    private void HandleInputState(Input<T> input)
    {
        input.OnHandle -= PopInput;
        
        if (Handle)
            input.SetHandled();
    }
}