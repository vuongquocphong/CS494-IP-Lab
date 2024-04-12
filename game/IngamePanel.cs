using Godot;
using System;
using GameComponents;
public partial class IngamePanel : Panel
{
	bool GuessCharMode;
	bool NotifyNewTurnInUpdateGameStatus;

	public void Reset(){
		// reset the player list
		ItemList NameList = GetNode<ItemList>("GridContainer/VBoxNameContainer/ItemList");
		ItemList PointList = GetNode<ItemList>("GridContainer/VBoxPointContainer/ItemList");
		ItemList GuessList = GetNode<ItemList>("GridContainer/VBoxGuessContainer/ItemList");
		NameList.Clear();
		PointList.Clear();
		GuessList.Clear();
		// reset the hint
		GetNode<RichTextLabel>("TextPanel/HintPanel/HintLabel").Text = " ";
		// reset the keyword
		GetNode<Label>("TextPanel/KeywordPanel/KeywordLabel").Text = " ";
		// reset the guess result
		GetNode<Label>("GuessResultLabel").Text = " ";
		// reset the warning message
		GetNode<RichTextLabel>("WarningMessageLabel").Text = " ";
		// reset the guess edit text
		GetNode<LineEdit>("GuessEditText").Text = " ";
		// reset the guess mode
		GetNode<Button>("GuessModeButton").Text = "Guess Mode: Character";
		// reset the guess timer
		GetNode<Button>("GuessTimerButton").SetProcess(false);
		GetNode<Godot.Timer>("GuessTimerButton/GuessTimer").Stop();
		// reset the turn alert
		GetNode<RichTextLabel>("TurnAlertLabel").Text = " ";
		// reset the animation player
		GetNode<AnimationPlayer>("AnimationPlayer").Stop();
		// reset the notify new turn in update game status
		NotifyNewTurnInUpdateGameStatus = true;
	}
    public override void _Ready()
    {
        base._Ready();
		GuessCharMode = true;
		GetNode<LineEdit>("GuessEditText").MaxLength = 1;
		NotifyNewTurnInUpdateGameStatus = true;
		GetNode<Button>("GuessTimerButton").SetProcess(false);
    }

	public void SetInvalidMessage(string invalidMessage)
	{
		var WarningMessageLabel = GetNode<RichTextLabel>("WarningMessageLabel");
		var Timer = WarningMessageLabel.GetNode<Godot.Timer>("WarningMessageTimer");
		WarningMessageLabel.Text = invalidMessage;
		GetNode<AnimationPlayer>("AnimationPlayer").Play("NotifyWarningSubmission");
	}

	public void OnExitButtonPressed()
	{
		GetNode<Button>("GuessTimerButton").SetProcess(false);
		GetNode<Godot.Timer>("GuessTimerButton/GuessTimer").Stop();
		GameManager.GetInstance().EmitSignal(GameManager.SignalName.BackFromIngameToInputName);
	}
	public void OnModeButtonPressed()
	{
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
		GameManager gameManager = GameManager.GetInstance();
		if (Guess.Length == 0)
		{
			SetInvalidMessage("You must enter at least one character!");
			return;
		}
		else if (gameManager.LocalPlayerName != gameManager.CurrentPlayer.Name)
		{
			SetInvalidMessage("Wait until your turn!");
			return;
		}
		foreach (char character in Guess){
			if (character == ' '){
				SetInvalidMessage("The guess must not contain whitespace!");
				return;
			}
		}
		GetNode<Button>("GuessTimerButton").SetProcess(false);
		GetNode<Godot.Timer>("GuessTimerButton/GuessTimer").Stop();

		GameManager.GetInstance().Guess(GuessCharMode, Guess);
	}
	public void NotifyGuessResults(string Guess, string PlayerName){
		Messages.GuessResult guessResult = GameManager.GetInstance().guessResult;
		string Message = guessResult switch
		{
			Messages.GuessResult.Correct => "CORRECT!!",
			Messages.GuessResult.Incorrect => "WRONG!!",
			Messages.GuessResult.Duplicate => "WRONG!!",
			Messages.GuessResult.Invalid => "INVALID!!",
		};
		int index = 0;
		var GuessList = GetNode<ItemList>("GridContainer/VBoxGuessContainer/ItemList");
		foreach (PlayerInfo player in GameManager.GetInstance().PlayersList){
			if (PlayerName == player.Name){
				GuessList.SetItemText(index, Guess);
			}
			else{
				GuessList.SetItemText(index, " ");
			}
		}
		NotifyNewTurnInUpdateGameStatus = false;
		GetNode<Label>("GuessResultLabel").Text = Message;
		GetNode<AnimationPlayer>("AnimationPlayer").Play("NotifyGuessResult");
	}
	public void UpdateGameStatus(){
		var gameManager = GameManager.GetInstance();
		var HintLabel = GetNode<RichTextLabel>("TextPanel/HintPanel/HintLabel");
		var NameList = GetNode<ItemList>("GridContainer/VBoxNameContainer/ItemList");
		var PointList = GetNode<ItemList>("GridContainer/VBoxPointContainer/ItemList");
		var GuessList = GetNode<ItemList>("GridContainer/VBoxGuessContainer/ItemList");
		var KeywordLabel = GetNode<Label>("TextPanel/KeywordPanel/KeywordLabel");
		var PlayerList = gameManager.PlayersList;
		
		String NewKeyword = " ";
		for (int i = 0; i < gameManager.KeyWord.Length; i ++){
			NewKeyword += gameManager.KeyWord[i].ToString() + " ";
		}
		KeywordLabel.Text = NewKeyword;
		HintLabel.Text = gameManager.Hint;
		int index = 0;
		NameList.Clear();
		PointList.Clear();
		GuessList.Clear();
		foreach (PlayerInfo player in PlayerList){
			NameList.AddItem(player.Name, null, false);
			PointList.AddItem(player.Point.ToString(), null, false);
			GuessList.AddItem(" ", null, false);
			NameList.SetItemCustomFgColor(index, Color.FromHtml("#ffffff"));
			PointList.SetItemCustomFgColor(index, Color.FromHtml("#ffffff"));
			GuessList.SetItemCustomFgColor(index, Color.FromHtml("#ffffff"));
			
			if (player.Name == gameManager.CurrentPlayer.Name){
				NameList.SetItemCustomFgColor(index, Color.FromHtml("#ebb663"));
				PointList.SetItemCustomFgColor(index, Color.FromHtml("#ebb663"));
				GuessList.SetItemCustomFgColor(index, Color.FromHtml("#ebb663"));
			}
			else if (player.State == PlayerState.Disconnected){
				NameList.SetItemCustomFgColor(index, Color.FromHtml("#787777"));
				PointList.SetItemCustomFgColor(index, Color.FromHtml("#787777"));
				GuessList.SetItemCustomFgColor(index, Color.FromHtml("#787777"));
			}
			else if (player.State == PlayerState.Lost){
				NameList.SetItemCustomFgColor(index, Color.FromHtml("#b50000"));
				PointList.SetItemCustomFgColor(index, Color.FromHtml("#b50000"));
				GuessList.SetItemCustomFgColor(index, Color.FromHtml("#b50000"));
			}
			index++;
		}
		if (NotifyNewTurnInUpdateGameStatus) NotifyNewTurn();
	}
	public void NotifyNewTurn(){
		var gameManager = GameManager.GetInstance();
		String NotificationMessage = "NEW TURN!!";
		if (gameManager.LocalPlayerName == gameManager.CurrentPlayer.Name){
			NotificationMessage = "YOUR TURN!!";
		}
		var TurnAlertLabel = GetNode<RichTextLabel>("TurnAlertLabel");
		TurnAlertLabel.Text = NotificationMessage;
		var AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		AnimationPlayer.Play("notifyNewTurn");
	}

	public void OnAnimationPlayerAnimationFinished(string AnimationName) {
		if (AnimationName == "notifyNewTurn"){
			var gameManager = GameManager.GetInstance();
			if (gameManager.LocalPlayerName == gameManager.CurrentPlayer.Name){
				GetNode<Godot.Timer>("GuessTimerButton/GuessTimer").Start(20);
				GetNode<Button>("GuessTimerButton").SetProcess(true);
			}
		}
		else if (AnimationName == "NotifyGuessResult"){
			NotifyNewTurn();
		}
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
