namespace GIAT.Nodes.Input.Type;

using System;
using GIAT.Interface;

[Flags]
public enum PressState
{
    Start = 0b01,
    Stop = 0b10,
}

public readonly record struct PressInput(PressState state) : IInput;