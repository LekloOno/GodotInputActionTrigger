namespace GIAT.Nodes.Input.Handler;

using Godot;

using GIAT.Components.Input.Handler;
using GIAT.Nodes.Input.Type;
using GIAT.Nodes.Input.Buffer;
using GIAT.Interface;
using GIAT.Nodes.Input.Producer;

[GlobalClass, Tool]
public partial class PressInputHandler : InputHandler<PressInput>
{
    [Export]
    public PressBufferData PressBufferConfig
    {
        get => _pressBufferConfig;
        set 
            => _pressBufferConfig = value
                ?? new PressBufferData();
    }

    private PressBufferData _pressBufferConfig = new PressBufferData();

    private PressInputProducer _pressProducer;
    
    private PressMode _pressMode;
    [Export]
    public PressMode PressMode
    {
        get => _pressMode;
        set
        {
            if (_pressMode == value)
                return;

            _pressMode = value;
            SetProducer();
        }
    }

    private void SetProducer()
    {
        _pressProducer = _pressMode.Producer();
        _producer = _pressProducer;        
    }

    public ulong LastInputStart => _pressProducer.LastInputStart;
    public ulong LastInputStop => _pressProducer.LastInputStop;
    public bool Active => _pressProducer.Active;

    protected override IBuffer<PressInput> Build() =>
        new PressBuffer(Buffer.Build<PressInput>(), PressBufferConfig);

    public override void _Ready()
    {
        SetProducer();
        base._Ready();
    }
}