using GameComponents;
using Godot;
using System;

public partial class ScoreboardPanel : Panel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnQuitButtonPressed() {
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.FromScoreBoardToInputName);
	}
	private void OnRestartButtonPressed() {
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.FromScoreBoardToWaiting);
	}
}
