using Godot;

public class BVTextButton : Button
{
	[Export]
	public bool noArrow = false;
	[Export]
	public Color hoverGlowColor = new Color(1, 1.5f, 1);
	[Export]
	public Color hoverColor = new Color("#5aff70");
	[Export]
	public Color defaultColor = new Color(1,1,1);
	
	AudioStreamPlayer audioPlayer;
	AudioStream hoveraudio, selectaudio, denyaudio;
	public override void _Ready()
	{
		
		//hoveraudio = ResourceLoader.Load<AudioStream>("res://Assets/SFX/UI/UI_Hover.wav");
		selectaudio = ResourceLoader.Load<AudioStream>("res://Assets/SFX/UI/UI_Hover.wav");
		denyaudio = ResourceLoader.Load<AudioStream>("res://Assets/SFX/UI/UI_Denied.wav");

		audioPlayer = new AudioStreamPlayer();
		audioPlayer.Autoplay = false;
		AddChild(audioPlayer);
		Connect("focus_entered", this, "onEnter");
		Connect("focus_exited", this, "onExit");
		Connect("mouse_entered", this, "onEnterMouse");
		Connect("mouse_exited", this, "onExitMouse");
		Connect("pressed", this, "onClick");
		ChangeDefaultColor(defaultColor);
		ChangeDefaultHoverColor(hoverColor);
	}
	public void ChangeDefaultColor(Color c)
    {
        if (HasColorOverride("font_color"))
        {
			RemoveColorOverride("font_color");
        }
		AddColorOverride("font_color", c);

	}
	public void ChangeDefaultHoverColor(Color c)
    {
		if (HasColorOverride("font_color_hover"))
		{
			RemoveColorOverride("font_color_hover");
		}
		AddColorOverride("font_color_hover", c);
		if (HasColorOverride("font_color_focus"))
		{
			RemoveColorOverride("font_color_focus");
		}
		AddColorOverride("font_color_focus", c);
		if (HasColorOverride("font_color_pressed"))
		{
			RemoveColorOverride("font_color_pressed");
		}
		AddColorOverride("font_color_pressed", c);
	}

	private void onEnter()
	{
		if (Disabled) return;

		audioPlayer.Stream = hoveraudio;
		audioPlayer.Play();
		if(noArrow) return;
		Text = Text.Trim('>', ' ', '<');
		Text = "> " + Text + " <";
		Modulate = hoverGlowColor;

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
		Modulate = new Color(1,1,1);
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
