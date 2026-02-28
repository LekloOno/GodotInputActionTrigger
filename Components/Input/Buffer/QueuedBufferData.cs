namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;
using Godot;

[GlobalClass, Tool]
public partial class QueuedBufferData : SimpleBufferData
{
    [Export] public uint Size {get; private set;}

    public override IBuffer<T> Build<T>() =>
        new QueuedBuffer<T>(this);
}