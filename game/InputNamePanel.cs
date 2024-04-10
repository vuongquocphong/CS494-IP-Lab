using GameComponents;
using Godot;
using System;
using Mediator;
using Messages;

public partial class InputNamePanel : Panel
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

	private void OnPlayButtonPressed()
	{
		// Get the line edit
		LineEdit lineEdit = GetNode<LineEdit>("NameLineEdit");
		// Get the text
		string text = lineEdit.Text;
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.PlayButtonPressed, text);
	}
}
