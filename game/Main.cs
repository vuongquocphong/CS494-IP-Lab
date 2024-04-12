using GameComponents;
using Godot;
using Mediator;
using Messages;
using NetworkClient;

public partial class Main : Node
{
	GameManager GameManager = null!;
	INetworkClient NetworkClient = null!;
	IMediator Mediator {get; set;} = null!;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager = GameManager.GetInstance();
		AddChild(GameManager);
		
		// Subscribe to GameManager events
		GameManager.ConnectionSuccess += ConnectionSuccessHandler;
		GameManager.ConnectionFail += ConnectionFailHandler;
		GameManager.PlayerListUpdate += PlayerListUpdateHandler;
		GameManager.BackFromWaitingToInputName += BackFromWaitingToInputNameHandler;
		GameManager.BackFromIngameToInputName += BackFromIngameToInputNameHandler;
		GameManager.BackFromScoreBoardToInputName += BackFromScoreBoardToInputNameHandler;
		GameManager.PlayButtonPressed += PlayButtonPressedHandler;
		GameManager.ReadyButtonPressed += ReadyButtonPressedHandler;
		GameManager.StartGameReceive += StartGameReceiveHandler;
		GameManager.GameResultReceive += GameResultReceiveHandler;
		GameManager.GuessResultReceive += GuessResultReceiveHandler;
		GameManager.GameStatusReceive += GameStatusReceiveHandler;
		GameManager.BackFromScoreBoardToWaiting += BackFromScoreBoardToWaitingHandler;
		// Get GameManager Node
		NetworkClient = new TcpNetworkClient();
		Mediator = new MessagePasser(GameManager, NetworkClient);
		// Subscribe to ConnectionSuccess event
		LoadScenes();
	}
	public void PlayButtonPressedHandler(string name) {
		GameManager.LocalPlayerName = name;
		GameManager.GetInstance().RequestConnect(name);
	}

	private void ConnectionSuccessHandler() {
		// Show WaitingPanel
		GetNode<Panel>("WaitingPanel").Show();
		// Hide InputNamePanel
		GetNode<Panel>("InputNamePanel").Hide();
	}

	private void ConnectionFailHandler(string error) {
		Panel InputNamePanel = GetNode<Panel>("InputNamePanel");
		RichTextLabel ErrorLabel = InputNamePanel.GetNode<RichTextLabel>("InvalidMessageLabel");
		ErrorLabel.Text = error;
		ErrorLabel.VisibleCharacters = -1;
	}

	private void BackFromWaitingToInputNameHandler() {
		// Reset GameManager
		GameManager.Reset();
		// Show InputNamePanel
		GetNode<Panel>("InputNamePanel").Show();
		// Hide WaitingPanel
		GetNode<Panel>("WaitingPanel").Hide();
		// Reset Game UI
		GameUIReset();
		// Close connection
		NetworkClient.Close();
	}
	private void BackFromScoreBoardToInputNameHandler() {
		// Reset GameManager
		GameManager.Reset();
		// Show InputNamePanel
		GetNode<Panel>("InputNamePanel").Show();
		// Hide ScoreboardPanel
		GetNode<Panel>("ScoreboardPanel").Hide();
		// Reset Game UI
		GameUIReset();
		// Close connection
		NetworkClient.Close();
	}
	private void BackFromIngameToInputNameHandler(){
		// Reset GameManager
		GameManager.Reset();
		// Show InputNamePanel
		GetNode<Panel>("InputNamePanel").Show();
		// Hide IngamePanel
		GetNode<Panel>("IngamePanel").Hide();
		// Reset Game UI
		GameUIReset();
		// Close connection
		NetworkClient.Close();
	}
	private void BackButtonPressedHandler() {
		// Reset GameManager
		GameManager.Reset();
		// Show InputNamePanel
		GetNode<Panel>("InputNamePanel").Show();
		// Hide WaitingPanel
		GetNode<Panel>("WaitingPanel").Hide();
		// Reset Game UI
		GameUIReset();
		// Close connection
		NetworkClient.Close();
	}
	private void GameUIReset() {
		GetNode<InputNamePanel>("InputNamePanel").Reset();
		GetNode<WaitingPanel>("WaitingPanel").Reset();
		GetNode<IngamePanel>("IngamePanel").Reset();
		GetNode<ScoreboardPanel>("ScoreboardPanel").Reset();
	}
	private void PlayerListUpdateHandler() {
		// Get ItemList for Name and Status
		ItemList PlayerList = GetNode<ItemList>("WaitingPanel/GridContainer/VBoxContainerName/ItemList");
		ItemList StatusList = GetNode<ItemList>("WaitingPanel/GridContainer/VBoxContainerStatus/ItemList");
		// Clear PlayerList
		PlayerList.Clear();
		StatusList.Clear();
		// Add players name to PlayerList
		foreach (GameComponents.PlayerInfo player in GameManager.PlayersList) {
			GD.Print(player.Name);
			PlayerList.AddItem(player.Name);
			StatusList.AddItem(player.ReadyStatus ? "Ready" : "Not Ready");
		}
		// Update the number of ready players
		RichTextLabel ReadyPlayersLabel = GetNode<RichTextLabel>("WaitingPanel/ReadyPlayersLabel");
		ReadyPlayersLabel.Text = GameManager.PlayersList.FindAll(player => player.ReadyStatus).Count + " / " + GameManager.PlayersList.Count + " Players Ready";
	}

	private void ReadyButtonPressedHandler() {
		// Get the current ready status of the player
		bool readyStatus = GameManager.PlayersList.Find(player => player.Name == GameManager.LocalPlayerName).ReadyStatus;
		// Set the ready status of the player
		GameManager.PlayersList.Find(player => player.Name == GameManager.LocalPlayerName).ReadyStatus = !readyStatus;
		// Send the ready status to the server
		GameManager.GetInstance().SendReady(!readyStatus);
		// Change the UI to match the ready status
		Button ReadyButton = GetNode<Button>("WaitingPanel/ReadyButton");
		RichTextLabel WaitingText = GetNode<RichTextLabel>("WaitingPanel/WaitingText");
		if (readyStatus) {
			WaitingText.Text = "Waiting for other players ready to play...";
			ReadyButton.Text = "READY";
		}
		else {
			WaitingText.Text = "Press the button to get ready for game...";
			ReadyButton.Text = "UNREADY";
		}
		GD.Print("Send Ready Message");
	}
	
	private void BackFromScoreBoardToWaitingHandler() {
		// Send the ready status to the server
		GameManager.GetInstance().RequestConnect(GameManager.LocalPlayerName);
		// Change the UI to match the ready status
		Button ReadyButton = GetNode<Button>("WaitingPanel/ReadyButton");
		RichTextLabel WaitingText = GetNode<RichTextLabel>("WaitingPanel/WaitingText");
		WaitingText.Text = "Press the button to get ready for game...";
		ReadyButton.Text = "READY";
		// Show WaitingPanel
		GetNode<Panel>("WaitingPanel").Show();
		// Hide ScoreboardPanel
		GetNode<Panel>("ScoreboardPanel").Hide();
		// Hide IngamePanel
		GetNode<Panel>("IngamePanel").Hide();
	}

	private void StartGameReceiveHandler() 
	{
		// Show IngamePanel
		GetNode<Panel>("IngamePanel").Show();
		// Hide WaitingPanel
		GetNode<Panel>("WaitingPanel").Hide();
	}

	private void GameResultReceiveHandler() 
	{
		// Stop ingame process
		GetNode<IngamePanel>("IngamePanel").Reset();
		// Call update function in ScoreboardPanel
		GetNode<ScoreboardPanel>("ScoreboardPanel").SetGameResult();
		// Show ScoreboardPanel
		GetNode<Panel>("ScoreboardPanel").Show();
		// Hide IngamePanel
		GetNode<Panel>("IngamePanel").Hide();
		// NetworkClient.Close();
	}
	private void GuessResultReceiveHandler(string Guess, string PlayerName)
	{

		GetNode<IngamePanel>("IngamePanel").NotifyGuessResults(Guess, PlayerName);
	}
	private void GameStatusReceiveHandler(){
		GetNode<IngamePanel>("IngamePanel").UpdateGameStatus();
	}
	// Load Scenes
	private void LoadScenes() {
		PackedScene InputNameScene = GD.Load<PackedScene>("res://InputNamePanel.tscn");
		PackedScene WaitingScene = GD.Load<PackedScene>("res://WaitingPanel.tscn");
		PackedScene ScoreboardScene = GD.Load<PackedScene>("res://ScoreboardPanel.tscn");
		PackedScene IngameScene = GD.Load<PackedScene>("res://IngamePanel.tscn");

		// Add scenes to tree
		AddSceneToTree(InputNameScene);
		AddSceneToTree(WaitingScene);
		AddSceneToTree(ScoreboardScene);
		AddSceneToTree(IngameScene);

		// Hide all scenes except InputNameScene
		GetNode<Panel>("WaitingPanel").Hide();
		GetNode<Panel>("ScoreboardPanel").Hide();
		GetNode<Panel>("IngamePanel").Hide();
	}
	private void AddSceneToTree(PackedScene scene) {
		if (scene != null) {
			Node panel = scene.Instantiate();
			AddChild(panel);
			GD.Print(scene.ResourceName + " loaded");
		}
		else {
			GD.Print(scene.ResourceName + " not found");
		}
	}
}
