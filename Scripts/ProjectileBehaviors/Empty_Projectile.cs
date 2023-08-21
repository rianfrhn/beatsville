using Godot;
using System;

public class Empty_Projectile : Projectile
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		QueueFree();
	}
	public override void Move(int beat)
	{


	}
}
