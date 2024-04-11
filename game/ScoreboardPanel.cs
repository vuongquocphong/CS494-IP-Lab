using GameComponents;
using Godot;
using System;

public partial class ScoreboardPanel : Panel
{
	public void SetGameResult(){
		var PlayersList = GameManager.GetInstance().PlayersList;
		var NameList = GetNode<ItemList>("GridContainer/PlayerNames");
		var PointList = GetNode<ItemList>("GridContainer/PlayerScores");
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
