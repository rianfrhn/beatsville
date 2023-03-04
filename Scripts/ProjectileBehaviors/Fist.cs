 using Godot;
using System;

public class Fist : Projectile
{
	public override void _Ready()
	{
		
		animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animplayer.Play("Step1");
		animplayer.Connect("animation_finished", this, "onAnimFinished");
		Vector2 direction = targetpoint - initialPos;

		Position += new Vector2(Mathf.Sign(direction.x)*16, Mathf.Sign(direction.y)*16);
	}
	public override void Move(int beat)
	{
		
	}
	private void onAnimFinished(string anim_name)
	{
		if (anim_name == "Step1") QueueFree();
	}
		



}
