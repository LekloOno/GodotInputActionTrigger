namespace GIAT.Nodes.Input.Type;

using System;

[Flags]
public enum PressInput
{
    Start = 0b01,
    Stop = 0b10,
}