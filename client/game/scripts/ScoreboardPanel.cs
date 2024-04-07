using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public partial class ScoreboardPanel : Panel
{
    private List<(string, int)> players = [];
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called when the QUIT button is pressed.
    private void OnQuitButtonPressed()
    {
        // Add your quit functionality here
        GetTree().Quit(); // This quits the game
    }

    // Called when the RESTART button is pressed.
    private void OnRestartButtonPressed()
    {
        // Add your restart functionality here
        // For example, you can return to the lobby scene
        GetTree().ChangeSceneToFile("res://waiting_panel.tscn"); // Change to your lobby scene path
    }
    public void SetPlayers(List<(string, int)> players)
    {
        this.players = players;
        var vboxname = GetNode<VBoxContainer>("VBoxName");
        var vboxscore = GetNode<VBoxContainer>("VBoxScore");
        foreach (var player in players)
        {
            var name = new Label
            {
                Text = player.Item1
            };
            vboxname.AddChild(name);
            var score = new Label
            {
                Text = player.Item2.ToString()
            };
            vboxscore.AddChild(score);
        }

    }
}
