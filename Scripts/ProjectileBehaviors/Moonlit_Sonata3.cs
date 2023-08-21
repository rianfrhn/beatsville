using Godot;
using System;

public class Moonlit_Sonata3 : Projectile
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		onCollideAutoDel = false;
		if(targetpoint.x < initialPos.x) Scale = new Vector2(-1, 1);
		animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animplayer.PlaybackSpeed = BV.GM.songBPM / 60.0f;
		animplayer.Play("Step1");
		animplayer.Connect("animation_finished", this, "onAnimFinished");
	}
	public override void Move(int beat)
	{


	}
	private void onAnimFinished(string anim_name)
	{
		QueueFree();
	}
}
