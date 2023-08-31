using Godot;
using System;

public class Menu : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }

	private void _on_Play_pressed()
	{
		// Replace with function body.
		GetTree().Root.GetNode<GlobalHandler>("GlobalHandler").NewGameData();
		GetTree().Root.GetNode<SceneTransition>("SceneTransition").ChangeScene("res://Scenes/UI/Cutscenes/Cutscene_Initial.tscn", "STOP");
		
	}
	private void _on_Continue_pressed()
	{
		GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
		gv.LoadGameData();
	}
	


	private void _on_Options_pressed()
	{
		// Replace with function body.
	}
	
	private void _on_Test_Map_pressed()
	{
		GetTree().Root.GetNode<GlobalHandler>("GlobalHandler").NewTestGameData();
		GetTree().Root.GetNode<SceneTransition>("SceneTransition").ChangeScene("res://Scenes/UI/Cutscenes/Cutscene_TestMap.tscn", "STOP");
	}
}






