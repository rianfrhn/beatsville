using Godot;
using System;

public class SceneTransition : CanvasLayer
{
	[Export]
	public Color defaultColor = new Color("ffffff");
	[Signal]
	public delegate void STDone();

	ColorRect rect;
	public override void _Ready()
	{
		BV.ST = this;
		rect = GetNode<ColorRect>("ColorRect");
	}
	public async void ChangeScene(string scenepath, string musicpath = "", string dialogue = "")
	{
		GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
		PackedScene s = ResourceLoader.Load<PackedScene>(scenepath, noCache: true);
		Node node = s.Instance();
		DefaultMusic defaultMusic = node.GetNodeOrNull<DefaultMusic>("DefaultMusic");
		GlobalMusic globalmusic = GetTree().Root.GetNode<GlobalMusic>("GlobalMusic");
		//Fade in
		defaultColor.a = 1.0f;
		ChangeScreenColor(defaultColor, 1.0f);

		//Change Music
		globalmusic.FadeOff();

		await ToSignal(this, "STDone");
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
		defaultColor.a = 0;
		ChangeScreenColor(defaultColor, 1.0f);
		node.QueueFree();
	}
	public void ChangeScreenColor(Color color, float duration)
	{
		SceneTreeTween st = GetTree().CreateTween();
		st.TweenProperty(rect, "modulate", color, duration);
		st.TweenCallback(this, "EmitOnSceneTreeDone");
	}
	public void ChangeScreenColorByString(string name)
	{
		Color col = new Color(0,0,0);
		switch (name.ToLower())
		{
			case "black":
				col = Colors.Black; break;
			case "white":
				col = Colors.White; break;
			case "red":
				col = Colors.Red; break;
		}
		GD.Print("Switching color to " + name + col);
		rect.Modulate = new Color(col.r, col.g, col.b, rect.Modulate.a);
	}

	public void ChangeScreenAlpha(float alpha, float duration)
	{
		SceneTreeTween st = GetTree().CreateTween();
		st.TweenProperty(rect, "modulate:a", alpha, duration);
		GD.Print("Changing alpha to " + alpha + " duration " + duration);
		st.TweenCallback(this, "EmitOnSceneTreeDone");
	}
	public void EmitOnSceneTreeDone()
	{
		EmitSignal("STDone");
	}


}
