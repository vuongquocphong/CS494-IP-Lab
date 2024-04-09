using GameComponents;
using Godot;
using System;
using System.Data;

public partial class WaitingPanel : Panel
{

	private bool Ready = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnReadyButtonPressed()
	{
		GameManager.GetInstance().PlayerReady(!Ready);
		Ready = !Ready;
	}
	private static void OnBackButtonPressed()
	{
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.BackToInputName);
	}

	public void UpdateReadyPlayerList(){
		var PlayerList = GameManager.GetInstance().PlayersList;
		var PlayerNameList = GetNode<ItemList>("PlayerNameList");
		var PlayerStateList = GetNode<ItemList>("PlayerStateList");
		var ReadyPlayersLabel = GetNode<RichTextLabel>("ReadyPlayersLabel");
		int count = 0;
		foreach (PlayerInfo player in PlayerList){
			PlayerNameList.AddItem(player.Name, null, false);
			PlayerStateList.AddItem(
				player.ReadyStatus switch
				{
					true => "READY",
					false => "NOT READY",
				},
				null, false
			);
			if (player.ReadyStatus) count++;
		}
		ReadyPlayersLabel.Text = count + "/" + PlayerList.Count() + " players ready";
	}
}
