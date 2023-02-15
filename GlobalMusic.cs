using Godot;
using System;

public class GlobalMusic : Node
{
	int playing = 1;
	AudioStreamPlayer as1;
	AudioStreamPlayer as2;
	AnimationPlayer animplayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		as1 = GetNode<AudioStreamPlayer>("AudioStreamPlayer1");
		as2 = GetNode<AudioStreamPlayer>("AudioStreamPlayer2");
		animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	public void SwitchMusic(string musicdir)
	{
		AudioStream track = ResourceLoader.Load<AudioStream>(musicdir);
		if(playing == 1)
		{
			as2.Stream = track;
			animplayer.Play("CrossTo2");
			playing = 2;
		}
		else
		{
			as1.Stream = track;
			animplayer.Play("CrossTo1");
			playing = 1;
		}
	}
	public void Reset()
	{
		animplayer.Play("RESET");
	}
}
