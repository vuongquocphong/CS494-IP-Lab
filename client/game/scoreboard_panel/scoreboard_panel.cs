using Godot;
using System;

public partial class scoreboard_panel : Panel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hide the panel
		this.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// If game state is end game, show the panel overlay the current scene
	}
}
