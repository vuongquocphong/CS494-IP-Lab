using GameComponents;
using Godot;
using System;
using Mediator;
using Messages;

public partial class InputNamePanel : Panel
{

	private void OnPlayButtonPressed()
	{
		// Get the line edit
		LineEdit lineEdit = GetNode<LineEdit>("NameLineEdit");
		// Get the text
		string text = lineEdit.Text;
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.PlayButtonPressed, text);
	}
}
