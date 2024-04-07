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
		
    }

	public void OnReady(){
		
		eventPasser = new EventPasser(GetTree());
	}
}
