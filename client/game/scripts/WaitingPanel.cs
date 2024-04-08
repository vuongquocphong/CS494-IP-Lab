using Godot;
using System;
using Mediator;
public partial class WaitingPanel : Panel
{
	private Boolean Ready;

	private Int32 Index;
	private Int32 NumberOfReadyPlayers;

	private EventPasser eventPasser;

    public override void _Ready()
    {
        base._Ready();
		eventPasser = EventPasser.GetInstance(GetTree());
		eventPasser.WaitingComp.ComponentNode = this;
    }

    // private List<Tuple<String, Boolean>> PlayerList = new List<Tuple<string, bool>>();
    public void OnReadyButtonPressed()
	{
		var ReadyButton = GetNode<Button>("ReadyButton");
		
		// MediatorComp.Notify(this, Event.READY);
		if (!Ready){
			NumberOfReadyPlayers -= 1;
			ReadyButton.Text = "UNREADY";
			Console.WriteLine(Ready);
		}
		else{
			NumberOfReadyPlayers += 1;
			ReadyButton.Text = "READY";
			Console.WriteLine(Ready);
		}
		Ready = !Ready;
		// String name = PlayerList[Index].Item1;
		// PlayerList[Index] = new Tuple<string, bool>(name, Ready);

	}

	public void OnBackButtonPressed()
	{
		
		// MediatorComp.Notify(this, Event.DISCONNECT);
	}
}
