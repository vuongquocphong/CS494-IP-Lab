using Godot;
using System;
using System.Data;
using Mediator;
using Messages;
using GameComponents;
public partial class InputNamePanel : Panel
{
	private EventPasser eventPasser;
    public override void _Ready()
    {
        base._Ready();
		eventPasser = EventPasser.GetInstance(GetTree());
		eventPasser.InputNameComp.ComponentNode = this;
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
		eventPasser.InputNameComp.OnPlayButtonPressed(GetName());
	}

	public void OnReceiveMessage(String Keyword){

	}
	
}
