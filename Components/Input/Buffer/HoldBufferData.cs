namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;
using Godot;

[GlobalClass, Tool]
public partial class HoldBufferData : SimpleBufferData
{
    public override IBuffer<T> Build<T>()
        => new HoldBuffer<T>(new SimpleBuffer<T>(this));
}