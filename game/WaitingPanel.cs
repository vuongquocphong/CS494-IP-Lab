using GameComponents;
using Godot;
using System;
using System.Data;

public partial class WaitingPanel : Panel
{	
	public void Reset(){
		// reset the player list
		ItemList PlayerList = GetNode<ItemList>("GridContainer/VBoxContainerName/ItemList");
		ItemList StatusList = GetNode<ItemList>("GridContainer/VBoxContainerStatus/ItemList");
		PlayerList.Clear();
		StatusList.Clear();
	}
	private void OnReadyButtonPressed()
	{
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.ReadyButtonPressed);
	}
	private static void OnBackButtonPressed()
	{
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.BackFromWaitingToInputName);
	}
}
