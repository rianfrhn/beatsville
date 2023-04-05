using Godot;
using System;

public class Cutscene : CanvasLayer
{
	[Export]
	public bool fromSaved = false;
	[Export]
	public string sceneDirectory;
	[Export]
	public string musicDirectory;
	
	AnimationPlayer animPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animPlayer.Play("Animation");
		animPlayer.Connect("animation_finished", this, "onAnimFinished");
	}
	void onAnimFinished(string animation)
	{
		if(fromSaved)
		{
			GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
			GetTree().Root.GetNode<SceneTransition>("SceneTransition").ChangeScene(gv.saveData.sceneDirectory, gv.saveData.musicDirectory);
			
		} else{
			GetTree().Root.GetNode<SceneTransition>("SceneTransition").ChangeScene(sceneDirectory, musicDirectory);
		}
	}
}
