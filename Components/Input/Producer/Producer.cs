namespace GIAT.Components.Input.Producer;

using Godot;

using GIAT.Interface;

public abstract class Producer<T> : Producer<InputEvent, T>, IProducer<T> {}