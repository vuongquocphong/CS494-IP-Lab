using Godot;
using System;
using GameComponents;
public partial class IngamePanel : Panel
{
	
	bool GuessCharMode = true;
	public void NewGame(){
		var gameManager = GameManager.GetInstance();
		var HintLabel = GetNode<RichTextLabel>("TextPanel/HintPanel/HintLabel");
		var NameList = GetNode<ItemList>("GridContainer/VBoxNameContainer/ItemList");
		var PointList = GetNode<ItemList>("GridContainer/VBoxPointContainer/ItemList");
		var GuessList = GetNode<ItemList>("GridContainer/VBoxGuessContainer/ItemList");
		var KeywordLabel = GetNode<Label>("TextPanel/KeywordPanel/KeywordLabel");
		var GuessTimerButton = GetNode<Button>("GuessTimerButton");
		var PlayerList = gameManager.PlayersList;
		
		String NewKeyword = " ";
		for (int i = 0; i < gameManager.KeyWord.Length; i ++){
			NewKeyword += "_ ";
		}
		KeywordLabel.Text = NewKeyword;
		HintLabel.Text = gameManager.Hint;
		foreach (PlayerInfo player in PlayerList){
			NameList.AddItem(player.Name);
			PointList.AddItem(player.Point.ToString());
			GuessList.AddItem(" ");
		}
		GuessTimerButton.SetProcess(false);
		NotifyNewTurn();
	}
	public void SetInvalidMessage(string invalidMessage)
	{
		var WarningMessageLabel = GetNode<RichTextLabel>("WarningMessageLabel");
		var Timer = WarningMessageLabel.GetNode<Godot.Timer>("WarningMessageTimer");
		WarningMessageLabel.Text = invalidMessage;
		WarningMessageLabel.VisibleCharacters = -1;
		Timer.Start(10);
	}

	public void OnExitButtonPressed()
	{
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.BackFromIngameToInputName);
	}
	public void OnModeButtonPressed()
	{
		if (GameManager.GetInstance().NumberOfTurns <= 2){

		}
		var GuessModeButton = GetNode<Button>("GuessModeButton");
		var GuessEditText = GetNode<LineEdit>("GuessEditText");
		if (GuessCharMode){
			GuessModeButton.Text = "Guess Mode: Keyword";
			GuessEditText.MaxLength = 0;
		}
		else{
			GuessModeButton.Text = "Guess Mode: Character";
			GuessEditText.MaxLength = 1;
		}
		GuessCharMode = !GuessCharMode;
	}

	public void OnSubmitButtonPressed()
	{
		var GuessEditText = GetNode<LineEdit>("GuessEditText");
		string Guess = GuessEditText.Text;
		if (Guess.Length == 0)
		{
			SetInvalidMessage("You must enter at least one character!");
			return;
		}
		foreach (char character in Guess){
			if (character == ' '){
				SetInvalidMessage("The guess must not contain whitespace!");
				return;
			}
		}
		GameManager.GetInstance().Guess(GuessCharMode, Guess);
		
	}
	public void UpdatePlayerList(){

	}
	public void OnWarningMessageTimerTimeout(){
		var WarningMessageLabel = GetNode<RichTextLabel>("WarningMessageLabel");
		WarningMessageLabel.VisibleCharacters = 0;
	}
	public void NotifyNewTurn(){
		var gameManager = GameManager.GetInstance();
		String NotificationMessage = "NEXT TURN!!";
		if (gameManager.LocalPlayerName == gameManager.CurrentPlayer.Name){
			NotificationMessage = "YOUR TURN!!";
		}
		var TurnAlertLabel = GetNode<RichTextLabel>("TurnAlertLabel");
		TurnAlertLabel.Text = NotificationMessage;
		var AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		AnimationPlayer.Play("notifyNewTurn");
	}

	public void OnAnimationPlayerAnimationFinished() {
		GetNode<Godot.Timer>("GuessTimerButton/GuessTimer").Start(20);
		GetNode<Button>("GuessTimerButton").SetProcess(true);
	}
	public void OnGuessTimerTimeout(){
		GetNode<Button>("GuessTimerButton").SetProcess(false);
		var GuessEditText = GetNode<LineEdit>("GuessEditText");
		string Guess = GuessEditText.Text;
		if (Guess.Length == 0)
		{
			GameManager.GetInstance().TimeOut();
			return;
		}
		foreach (char character in Guess){
			if (character == ' '){
				GameManager.GetInstance().TimeOut();
				return;
			}
		}
		var ItemList = new ItemList();
		ItemList.AddItem(Guess);
		GameManager.GetInstance().Guess(GuessCharMode, Guess);
	}
	
}
