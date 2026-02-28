namespace GIAT.Components.Input.Buffer;

using GIAT.Interface;
using Godot;

[GlobalClass]
public abstract partial class BufferData: Resource
{
    public abstract IBuffer<T> Build<T>();
}