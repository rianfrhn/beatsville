using Godot;
using System;

public class StateLabel : Label
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Humanoid player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent<Humanoid>();
	}


	public override void _Process(float delta)
	{
		switch (player.currentState)
		{
			case Humanoid.status.Idle:
				Text = "Idle";
				break;
			case Humanoid.status.Moving:
				Text = "Moving";
				break;
			case Humanoid.status.Stunned:
				Text = "Stunned";
				break;
			case Humanoid.status.Interacting:
				Text = "Interacting";
				break;
			case Humanoid.status.Attacking:
				Text = "Attacking";
				break;
			default: break;



		}
	}
}
