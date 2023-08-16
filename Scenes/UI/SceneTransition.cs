using Godot;
using System;

public class SceneTransition : CanvasLayer
{
	[Export]
	public Color defaultColor = new Color("ffffff");
	public override void _Ready()
	{
		BV.ST = this;
	}
	public async void ChangeScene(string scenepath, string musicpath = "", string dialogue = "")
	{
		GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
		PackedScene s = ResourceLoader.Load<PackedScene>(scenepath, noCache: true);
		Node node = s.Instance();
		DefaultMusic defaultMusic = node.GetNodeOrNull<DefaultMusic>("DefaultMusic");

		ColorRect rect = GetNode<ColorRect>("ColorRect");
		rect.Color = defaultColor;
		GlobalMusic globalmusic = GetTree().Root.GetNode<GlobalMusic>("GlobalMusic");



		AnimationPlayer animplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		//Fade in
		animplayer.Play("Dissolve");

		//Change Music
		globalmusic.FadeOff();

		await ToSignal(animplayer, "animation_finished");
		GetTree().ChangeScene("res://Scenes/Map/Limbo.tscn");
		if (dialogue !="" && dialogue != null)
		{
			Node dialogueNode = DialogicSharp.Start(dialogue);
			AddChild(dialogueNode);
			await ToSignal(dialogueNode, "timeline_end");
		}
		GetTree().ChangeScene(scenepath);

		string mp = "";
		//Turn on music
		if((musicpath == null || musicpath == "") && defaultMusic != null && (defaultMusic.musicPath != null || defaultMusic.musicPath != ""))
        {
			mp = defaultMusic.musicPath;
        } else if(musicpath != null || musicpath != "")
        {
			mp = musicpath;
        }
		globalmusic.On(mp);

		if (node is MapScene)
		{
			gv.currentSceneDir = scenepath;
		}
		animplayer.PlayBackwards("Dissolve");
		node.QueueFree();
	}


}
