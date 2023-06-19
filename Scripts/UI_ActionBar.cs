using Godot;
using System;
using Godot.Collections;

public class UI_ActionBar : Node
{
	int beats;
	TextureRect template;
	public override void _Ready()
	{
		Initialize();
	}
	public void Initialize()
    {
		template = GetNode<TextureRect>("Template");
		beats = BV.GM.songData.TimeSign;
		AnimationPlayer animplr = GetNode<AnimationPlayer>("AnimationPlayer");
		Animation anim = animplr.GetAnimation("Jump");
		GD.Print(anim.TrackGetKeyValue(0, 0).GetType());

		for (int i = 0; i < beats; i++)
		{
			TextureRect act = (TextureRect)template.Duplicate();
			act.Name = i.ToString();
			GD.Print(i.ToString());
			act.RectPosition = new Vector2((beats - i) * -16f, -16);
			AddChild(act);
			AnimationPlayer animationPlayer = new AnimationPlayer();
			animationPlayer.Name = "AnimationPlayer";
			animationPlayer.AddAnimation("Jump", CreateAnimation(act));
			act.AddChild(animationPlayer);
		}
		BV.GM.Connect("EmitBeat", this, "Jump");
		template.QueueFree();

	}
	public void Jump(int beat)
	{
		int b = beat % BV.GM.songData.TimeSign;
		TextureRect texture = GetNode<TextureRect>(b.ToString());
		AnimationPlayer animplayer = texture.GetNode<AnimationPlayer>("AnimationPlayer");
		animplayer.Play("Jump");
		if (BV.GM.songData.beatsToAtk.Contains(b))
		{
			texture.Modulate = new Color(0, 1, 0);
		} else if (BV.GM.songData.beatsToMove.Contains(b))
		{
			texture.Modulate = new Color(0, 0, 1);
		}
		else
		{
			texture.Modulate = new Color(1, 0, 0);
		}

	}
	Animation CreateAnimation(TextureRect rect)
	{
		string path = rect.GetPath() + ":rect_position:y";
		Animation anim = new Animation();
		int trackNum = anim.AddTrack(Animation.TrackType.Value, 0);
		float len = 60 / BV.GM.songBPM;
		anim.Length = len;
		anim.TrackSetPath(trackNum, path);
		anim.TrackInsertKey(trackNum, 0, -16);
		anim.TrackInsertKey(trackNum, len/2, -16-8);
		anim.TrackInsertKey(trackNum, len, -16);
		//anim.ValueTrackSetUpdateMode(trackNum, Animation.UpdateMode.Continuous);
		anim.Loop = false;
		return anim;
	}


}
