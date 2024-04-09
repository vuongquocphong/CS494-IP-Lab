using GameComponents;
using Godot;
using System;
using Mediator;
using Messages;

public partial class InputNamePanel : Panel
{
    [Signal]
    public delegate void PlayButtonPressedEventHandler(string name);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		PlayButtonPressed += PlayButtonPressedHandler;
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
		if (CheckValidName(text))
		{
			EmitSignal(SignalName.PlayButtonPressed, text);
		}
		else
		{
			// Print an error message
			GD.Print("Invalid name");
		}
	}

	static public void PlayButtonPressedHandler(string name) {
		GameManager.GetInstance().RequestConnect(name);
	}

	private bool CheckValidName(string name)
	{
		// Check if the name is valid
		if (name.Length > 2 && name.Length < 10)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
