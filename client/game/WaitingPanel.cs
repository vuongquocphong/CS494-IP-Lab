using Godot;
using System;
using Mediator;
public partial class WaitingPanel : Panel
{
	private IMediator MediatorComp;
	private Boolean Ready;

	private Int32 Index;
	private Int32 NumberOfReadyPlayers;

    public override void _Ready()
    {
        base._Ready();
		Ready = false;
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
		
		GetTree().ChangeSceneToFile("res://InputNamePanel.tscn");
	}
}
