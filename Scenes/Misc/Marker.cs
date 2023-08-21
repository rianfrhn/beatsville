using Godot;
using System;

public class Marker : Sprite
{
	public override void _Ready()
	{
		AnimationPlayer animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animPlayer.Play("Initial");
		animPlayer.Connect("animation_finished", this, "on_anim_finished");
		
	}
	public void on_anim_finished(string _anim_name){
		QueueFree();
	}
}
