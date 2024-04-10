using GameComponents;
using Godot;
using System;
using System.Data;

public partial class WaitingPanel : Panel
{	private void OnReadyButtonPressed()
	{
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.ReadyButtonPressed);
	}
	private static void OnBackButtonPressed()
	{
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.BackFromWaitingToInputName);
	}
}
