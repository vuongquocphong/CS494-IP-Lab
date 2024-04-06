using Godot;
using System;
using Mediator;
using System.Runtime.CompilerServices;
public partial class Main : Node
{
	// Load all classes and datas
	private EventPasser eventPasser;
    public override void _Ready()
    {
        base._Ready();
		GetTree().ChangeSceneToFile("res://InputNamePanel.tscn");
		eventPasser = new EventPasser(GetTree());
    }
}
