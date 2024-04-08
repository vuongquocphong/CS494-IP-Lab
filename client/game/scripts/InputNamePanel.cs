using Godot;
using System;
using System.Data;
using Mediator;
using GameComponents;

namespace Components
{
	public partial class InputNamePanel(IMediator mediator) : Panel, IComponent
	{
		private EventPasser eventPasser = (EventPasser)mediator;

        public IMediator Mediator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void _Ready()
		{
			base._Ready();
			eventPasser = EventPasser.GetInstance(GetTree());
			eventPasser.InputNameComp = this;
		}
		public void SetInvalidMessage(String invalidMessage)
		{
			var InvalidMessageNode = GetNode<RichTextLabel>("InvalidMessageLabel");
			InvalidMessageNode.Text = invalidMessage;
			InvalidMessageNode.VisibleCharacters = -1;
		}
		public String GetName()
		{
			return GetNode<LineEdit>("NameLineEdit").Text;
		}
		public void OnPlayButtonPressed()
		{
			eventPasser.InputNameComp.OnPlayButtonPressed(GetName());
		}

		public void OnReceiveMessage(String Keyword)
		{

		}

	}

}
