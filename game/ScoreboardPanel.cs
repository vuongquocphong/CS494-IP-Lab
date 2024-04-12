using GameComponents;
using Godot;
using System;

public partial class ScoreboardPanel : Panel
{
	public void Reset(){
		// reset the player list
		ItemList PlayerList = GetNode<ItemList>("GridContainer/PlayerNames");
		ItemList StatusList = GetNode<ItemList>("GridContainer/PlayerScores");
		PlayerList.Clear();
		StatusList.Clear();
	}
	public void SetGameResult(){
		var PlayersList = GameManager.GetInstance().PlayersList;
		var NameList = GetNode<ItemList>("GridContainer/PlayerNames");
		var PointList = GetNode<ItemList>("GridContainer/PlayerScores");
		NameList.Clear();
		PointList.Clear();
		foreach (PlayerInfo player in PlayersList){
			NameList.AddItem(player.Name);
			PointList.AddItem(player.Point.ToString());
		}
	}
	private void OnQuitButtonPressed() {
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.BackFromScoreBoardToInputName);
	}
	private void OnRestartButtonPressed() {
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.BackFromScoreBoardToWaiting);
	}
}
