using Godot;
using System;

public class BVTextButton : Button
{
	[Export]
	public bool noArrow = false;
	
	AudioStreamPlayer audioPlayer;
	AudioStream hoveraudio, selectaudio, denyaudio;
	public override void _Ready()
	{
		
		hoveraudio = ResourceLoader.Load<AudioStream>("res://Assets/SFX/UI/UI_Hover.wav");
		selectaudio = ResourceLoader.Load<AudioStream>("res://Assets/SFX/UI/UI_Select.wav");
		denyaudio = ResourceLoader.Load<AudioStream>("res://Assets/SFX/UI/UI_Denied.wav");

		audioPlayer = new AudioStreamPlayer();
		audioPlayer.Autoplay = false;
		AddChild(audioPlayer);

		Connect("focus_entered", this, "onEnter");
		Connect("focus_exited", this, "onExit");
		Connect("mouse_entered", this, "onEnterMouse");
		Connect("mouse_exited", this, "onExitMouse");
		Connect("pressed", this, "onClick");
	}

	private void onEnter()
	{
		if (Disabled) return;

		audioPlayer.Stream = hoveraudio;
		audioPlayer.Play();
		if(noArrow) return;
		Text = Text.Trim('>', ' ', '<');
		Text = "> " + Text + " <";
	}
	private void onClick()
	{
		if (Disabled)
		{
			audioPlayer.Stream = denyaudio;
		}
		else
		{
			audioPlayer.Stream = selectaudio;
		}
		audioPlayer.Play();
	}
	private void onExit()
	{
		if(noArrow) return;
		Text = Text.Trim('>', ' ', '<');
	}
	private void onEnterMouse()
	{
		GrabFocus();
	}
	private void onExitMouse()
	{
		//ReleaseFocus();
	}


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
