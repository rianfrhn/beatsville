using Godot;
using System;

public class TestRoom2 : MapScene
{
	public override void _Ready()
	{
		PlacePlayer();
		Node startDialogue = DialogicSharp.Start("ReiTest");
		AddChild(startDialogue);
		Node startDialogue2 = DialogicSharp.Start("ChimeraGrunt");
		AddChild(startDialogue2);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
