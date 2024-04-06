using Godot;
using System;
using System.Data;
using Mediator;
public partial class InputNamePanel : Panel
{
	private IMediator MediatorComp;
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

	public String GetName(){
		return GetNode<LineEdit>("NameLineEdit").Text;
	}
	public void OnPlayButtonPressed()
	{
		var name = GetNode<LineEdit>("NameLineEdit").Text;
		if (!IsValidName(name))
		{
			var InvalidMessageNode = GetNode<RichTextLabel>("InvalidMessageLabel");
			InvalidMessageNode.Text = "Invalid name, please try again!";
			InvalidMessageNode.VisibleCharacters = -1;
		}
		else
		{
			MediatorComp.Notify(this, Event.CONNECT);
		}
	}
	
}
