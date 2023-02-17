using Godot;
using System;

public class GlobalMusic : Node
{
	int playing = 0;
	AnimationPlayer animplayer;
	AudioStreamPlayer[] audioStreams = new AudioStreamPlayer[2];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		audioStreams[0] = GetNode<AudioStreamPlayer>("AudioStreamPlayer1");
		audioStreams[1] = GetNode<AudioStreamPlayer>("AudioStreamPlayer2");
		animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	public void SwitchMusic(string musicdir)
	{
		
		AudioStream track = ResourceLoader.Load<AudioStream>(musicdir);
		if(playing == 0)
		{
			audioStreams[1].Stream = track;
			animplayer.Play("CrossTo2");
			playing = 1;
		}
		else
		{
			audioStreams[0].Stream = track;
			animplayer.Play("CrossTo1");
			playing = 0;
		}
		GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
		gv.currentMusic = musicdir;
	}
	public void Reset()
	{
		animplayer.Play("RESET");
	}
	public void FadeOff()
	{
		animplayer.PlayBackwards("Fade" + (playing + 1));
	}
	public void FadeOn(string musicdir = "")
	{
		if(musicdir != "")
		{
			AudioStream track = ResourceLoader.Load<AudioStream>(musicdir);
			audioStreams[playing].Stream = track;
			audioStreams[playing].Play();
			GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
			gv.currentMusic = musicdir;
		}
		animplayer.Play("Fade" + (playing + 1));
	}
	public void On(string musicdir = "")
	{
		if (musicdir != "")
		{
			if (musicdir.ToLower() != "stop")
			{
				AudioStream track = ResourceLoader.Load<AudioStream>(musicdir);
				audioStreams[playing].Stream = track;
				GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
				gv.currentMusic = musicdir;
			}
			else
			{
				audioStreams[playing].Stream = null;
			}
		}
		audioStreams[playing].VolumeDb = 0;
		audioStreams[playing].Play();

	}
	public AudioStreamPlayer GetPlayingAudioStream()
	{
		return audioStreams[playing];
	}
}
