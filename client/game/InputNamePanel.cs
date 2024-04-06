using Godot;
using System;
using System.Data;
using Mediator;
using Messages;
using GameComponents;
public partial class InputNamePanel : Panel
{
	private IMediator MediatorComp;

	public InputNamePanel(){}
	
	public void SetMediator(IMediator mediatorComp){
		MediatorComp = mediatorComp;
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
		MediatorComp.Notify(this, Event.REQUEST_CONNECT);
	}

	public void OnReceiveMessage(String Keyword){

	}
	
}
