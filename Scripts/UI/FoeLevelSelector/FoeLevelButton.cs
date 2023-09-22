using Godot;
using System;

public class FoeLevelButton : BVTextButton
{
	[Export]
	public FightData easyDifficulty = null;
	[Export]
	public FightData normalDifficulty = null;
	[Export]
	public FightData hardDifficulty = null;
	[Export]
	public FightData extremeDifficulty = null;
	
	[Signal]
	public delegate void FoeSelected(string name, FightData easy, FightData normal, FightData hard, FightData extreme);
	public override void _Ready()
	{
		base._Ready();
		Connect("pressed", this, "EmitSelectedSignal");
	}
	public void EmitSelectedSignal()
	{
		EmitSignal("FoeSelected",Name, easyDifficulty, normalDifficulty, hardDifficulty, extremeDifficulty);
	}

}
