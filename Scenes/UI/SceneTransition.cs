using Godot;
using System;

public class SceneTransition : CanvasLayer
{
	Color defaultColor = new Color("51d254");
	public async void ChangeScene(string scenepath, string musicpath = "", string dialogue = "")
	{

		ColorRect rect = GetNode<ColorRect>("ColorRect");
		rect.Color = defaultColor;
		GlobalMusic globalmusic = GetTree().Root.GetNode<GlobalMusic>("GlobalMusic");



		AnimationPlayer animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		//Fade in
		animplayer.Play("Dissolve");

		//Change Music
		globalmusic.FadeOff();

		await ToSignal(animplayer, "animation_finished");
		if (dialogue !="" && dialogue != null)
		{
			Node dialogueNode = DialogicSharp.Start(dialogue);
			AddChild(dialogueNode);
			await ToSignal(dialogueNode, "timeline_end");
		}

		//Turn on music
		globalmusic.On(musicpath);

		GetTree().ChangeScene(scenepath);
		animplayer.PlayBackwards("Dissolve");
	}
	

}
