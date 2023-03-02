using Godot;
using System;

public class SkillForm : Resource
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";


	// Called when the node enters the scene tree for the first time.
	[Export]
	public PackedScene projectile;
	[Export]
	public int duration;
	[Export]
	public int cost;
	[Export]
	public int damage;
	[Export]
	public string desc;

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
