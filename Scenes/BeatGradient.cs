using Godot;
using System;

public class BeatGradient : TextureRect
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	SceneTreeTween st;
	public override void _Ready()
	{
		BV.GM.Connect("EmitBeat", this, "BeatPulse");
	}

	public void BeatPulse(int beat)
	{
		int b = beat % BV.GM.songData.TimeSign;
		if (BV.GM.songData.beatsToMove.Contains(b))
		{
			Pulse();
		}
	}
	public void Pulse()
	{
		if(st != null) { st.Kill();}
		st = GetTree().CreateTween();
		Modulate = new Color(1, 1, 1, 1);
		st.TweenProperty(this, "modulate:a", 0.0f, 1.0f);
	}
}
