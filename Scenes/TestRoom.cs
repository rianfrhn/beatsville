using Godot;
using System;

public class TestRoom : MapScene
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public string sceneDirectory = "res://Scenes/TestRoom.tscn";
	public override void _Ready()
	{
		PlacePlayer(sceneDirectory);
		GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
		//Node dialogueNode = DialogicSharp.Start("ShiroTest");
		//AddChild(dialogueNode);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
