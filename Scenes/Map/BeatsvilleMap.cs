using Godot;
using System;

public class BeatsvilleMap : MapScene
{
	public override void _Ready()
	{
		base._Ready();
		Humanoid cutter = BV.WM.CharacterEnter(References.Char.Cutter, new Vector2(1256,312), false);
		cutter.dialogue = "CutterFights";
		
	}
}
