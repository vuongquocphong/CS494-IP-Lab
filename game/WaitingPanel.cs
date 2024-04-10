using Godot;
using GameComponents;

public partial class WaitingPanel : Panel
{
	// [Signal]
	// public delegate void ReadyButtonPressedEventHandler();
	// [Signal]
	// public delegate void BackToInputNameEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		// ReadyButtonPressed += OnReadyButtonPressed;
		// BackToInputName += OnBackButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnReadyButtonPressed()
	{
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.ReadyButtonPressed);
	}
	private static void OnBackButtonPressed()
	{
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.BackToInputName);
	}
}
