using Godot;
using System;
using System.Data;
using Mediator;
using Messages;
using GameComponents;
public partial class InputNamePanel : Panel
{
	private Boolean IsValidName(String name)
	{
		if (name.Length <= 0 || name.Length > 10)
		{
			return false;
		}
		foreach (char character in name){
			if (
				(character < 'a' || character > 'z') &&
				(character < 'A' || character > 'Z') &&
				(character < '0' || character > '9') &&
				(character != '_')
			)
			{
				return false;
			}
		}
		return true;
	}

	public void SetInvalidMessage(String invalidMessage){
		var InvalidMessageNode = GetNode<RichTextLabel>("InvalidMessageLabel");
			InvalidMessageNode.Text = invalidMessage;
			InvalidMessageNode.VisibleCharacters = -1;
	}
	public String GetName(){
		return GetNode<LineEdit>("NameLineEdit").Text;
	}
	public void OnPlayButtonPressed()
	{
		var name = GetNode<LineEdit>("NameLineEdit").Text;
		if (!IsValidName(name))
		{
			SetInvalidMessage("Invalid name, please try again!");
		}
		else
		{
			GetTree().ChangeSceneToFile("res://WaitingPanel.tscn");
		}
	}
	
}
