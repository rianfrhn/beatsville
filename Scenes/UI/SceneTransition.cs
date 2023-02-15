using Godot;
using System;

public class SceneTransition : CanvasLayer
{
	public async void ChangeScene(string scenepath, string musicpath = "")
	{
		GlobalMusic globalmusic = GetTree().Root.GetNode<GlobalMusic>("GlobalMusic");
		if (musicpath == "STOP")
		{
			globalmusic.Reset();
		}else if (musicpath!="" && musicpath != null)
		{
			globalmusic.SwitchMusic(musicpath);
		}



		AnimationPlayer animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animplayer.Play("Dissolve");
		await ToSignal(animplayer, "animation_finished");
		GetTree().ChangeScene(scenepath);
		animplayer.PlayBackwards("Dissolve");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
