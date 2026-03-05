namespace GIAT.Components.Input.Buffer;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GIAT.Components.Input.Dispatcher;

public class EmptyBuffer<T> : BufferBase<T>
{
    protected override bool ConsumeSpec(out Input<T> input)
    {
        input = default;
        return false;   
    }

    public override bool IsEmpty()
        => true;

    public override bool Peek([MaybeNullWhen(false)] out Input<T> input)
    {   
        input = default;
        return false;
    }

    protected override void ClearSpec() {}

    protected override bool ConsumeSpec(Input<T> input)
        => false;

    protected override bool BufferSpec(Input<T> input)
        => false;

    protected override void PopInputSpec(Input<T> input){}

    protected override bool PopSpec([MaybeNullWhen(false)] out Input<T> input)
    {
        input = default;
        return false;
    }

    protected override List<Input<T>> GetInputs()
        => [];
}