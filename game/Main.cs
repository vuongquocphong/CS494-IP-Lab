using GameComponents;
using Godot;
using Mediator;
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
		GameManager.ConnectionSuccess += ConnectionSuccessHandler;
		GameManager.ConnectionFail += ConnectionFailHandler;
		GameManager.BackToInputName += BackToInputNameHandler;
		GameManager.UpdateReadyPlayerListSignal += UpdateReadyPlayerListHandler;
		// Get GameManager Node
		NetworkClient = new TcpNetworkClient();
		Mediator = new MessagePasser(GameManager, NetworkClient);
		// Subscribe to ConnectionSuccess event
		LoadScenes();
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
		ErrorLabel.Show();
	}

<<<<<<< Updated upstream
	private void BackToInputNameHandler() {
=======
	private void UpdateReadyPlayerListHandler() {
		GetNode<WaitingPanel>("WaitingPanel").UpdateReadyPlayerList();
	}
	private void BackButtonPressedHandler() {
>>>>>>> Stashed changes
		// Show InputNamePanel
		GetNode<Panel>("InputNamePanel").Show();
		// Hide WaitingPanel
		GetNode<Panel>("WaitingPanel").Hide();
		// Close connection
		NetworkClient.Close();
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
