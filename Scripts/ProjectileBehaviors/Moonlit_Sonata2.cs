using Godot;
using System;

public class Moonlit_Sonata2 : Projectile
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	bool cd = false;
	public override void _Ready()
	{
		onCollideAutoDel = false;
		CollisionShape2D collision = GetNode<CollisionShape2D>("CollisionShape2D");
		Vector2 vec = targetpoint - initialPos;
		collision.Position = new Vector2(Mathf.Abs(vec.x), vec.y);
		animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animplayer.PlaybackSpeed = BV.GM.songBPM / 60.0f;
		Timer cdTimer = GetNode<Timer>("Timer");
		cdTimer.Connect("timeout", this, "onTimeout");
		animplayer.Play("Step1");
		animplayer.Connect("animation_finished", this, "onAnimFinished");
	}
	public override void Move(int beat)
	{
		if (!cd) return;
		animplayer.Play("Step2");
		cd = true;






	}
	private void onTimeout()
	{
		cd = true;
	}
	private void onAnimFinished(string anim_name)
	{
		if (anim_name == "Step2") QueueFree();
	}
}
