using Godot;
using System;

public class Moonlit_Sonata1 : Projectile
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	int frame = 2;
	bool cd = false;
	public override void _Ready()
	{
		animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animplayer.PlaybackSpeed = BV.GM.songBPM / 60.0f;
		Particles2D absorbParticle = GetNode<Particles2D>("Absorb");
		absorbParticle.SpeedScale = BV.GM.songBPM / 60.0f;
		Timer cdTimer = GetNode<Timer>("Timer");
		cdTimer.Connect("timeout", this, "onTimeout");
		animplayer.Play("Step1");
		animplayer.Connect("animation_finished", this, "onAnimFinished");
	}
	public override void Move(int beat)
	{
		if (!cd) return;
		animplayer.Play("Step"+frame);
		GD.Print("Step" + frame);
		if (frame <= 2)
		{
			frame++;
		}






	}
	private void onTimeout()
	{
		cd = true;
	}
	private void onAnimFinished(string anim_name)
	{
		if (anim_name == "Step3") QueueFree();
	}
}
