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
		GetTree().Root.GetNode<GlobalVariable>("GlobalVariable").NewGameData();
		GetTree().Root.GetNode<SceneTransition>("SceneTransition").ChangeScene("res://Scenes/TestRoom.tscn", "res://Assets/OST/Dire Guardian.ogg");
		
	}
	private void _on_Continue_pressed()
	{
		GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
		gv.LoadGameData();
		GetTree().Root.GetNode<SceneTransition>("SceneTransition").ChangeScene(gv.saveData.sceneDirectory, gv.saveData.musicDirectory);
	}
	


	private void _on_Options_pressed()
	{
		// Replace with function body.
	}
}




