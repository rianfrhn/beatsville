using Godot;
using System;

public class HouseAdelle : MapScene
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		if (DialogicSharp.GetVariable("FoughtShadow") == "1")
		{ 

			BV.WM.CharacterEnter("res://Scenes/Interactables/NPC/Marione.tscn", new Vector2(168, 88), false);
			BV.WM.CharacterTeleport("Adelle", new Vector2(216, 56));
			BV.WM.CharacterTeleport("Player", new Vector2(232, 56));
			Node dialog = DialogicSharp.Start("AdelleCheckup");
			AddChild(dialog);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
