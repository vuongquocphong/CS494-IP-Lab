using GameComponents;
using Godot;
using System;
using Mediator;
using Messages;

public partial class InputNamePanel : Panel
{

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnPlayButtonPressed()
	{
		// Get the line edit
		LineEdit lineEdit = GetNode<LineEdit>("NameLineEdit");
		// Get the text
		string name = lineEdit.Text;
		GameManager.GetInstance().RequestConnect(name);
	}
}
