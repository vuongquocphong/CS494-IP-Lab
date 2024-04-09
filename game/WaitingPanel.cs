using Godot;
using System;

public partial class WaitingPanel : Panel
{
	[Signal]
	public delegate void ReadyButtonPressedEventHandler();
	[Signal]
	public delegate void BackButtonPressedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		ReadyButtonPressed += OnReadyButtonPressed;
		BackButtonPressed += OnBackButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnReadyButtonPressed()
	{
		throw new NotImplementedException();
	}
	private void OnBackButtonPressed()
	{
		CallDeferred("emit_signal", "BackButtonPressed");
	}
}
