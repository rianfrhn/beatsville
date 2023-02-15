using Godot;
using System;
using Godot.Collections;

public class UI_ActionBar : Node
{
	int beats;
	TextureRect template;
	public override void _Ready()
	{
		template = GetNode<TextureRect>("Template");
		beats = GlobalHandler.CurrentMusic.songTimeSig;
		AnimationPlayer animplr = GetNode<AnimationPlayer>("AnimationPlayer");
		Animation anim = animplr.GetAnimation("Jump");
		GD.Print(anim.TrackGetKeyValue(0,0).GetType());

		for(int i=0; i<beats; i++)
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
		GlobalHandler.CurrentMusic.Connect("EmitBeat", this, "Jump");
		template.QueueFree();
	}
	public void Jump(int beat)
	{
		int b = beat % GlobalHandler.CurrentMusic.songTimeSig;
		TextureRect texture = GetNode<TextureRect>(b.ToString());
		AnimationPlayer animplayer = texture.GetNode<AnimationPlayer>("AnimationPlayer");
		animplayer.Play("Jump");
		if (GlobalHandler.CurrentMusic.songData.beatsToAtk.Contains(b))
		{
			texture.Modulate = new Color(0, 255, 0);
		} else if (GlobalHandler.CurrentMusic.songData.beatsToMove.Contains(b))
		{
			texture.Modulate = new Color(0, 0, 255);
		}
		else
		{
			texture.Modulate = new Color(255, 0, 0);
		}

	}
	Animation CreateAnimation(TextureRect rect)
	{
		string path = rect.GetPath() + ":rect_position:y";
		Animation anim = new Animation();
		int trackNum = anim.AddTrack(Animation.TrackType.Value, 0);
		float len = 60 / GlobalHandler.CurrentMusic.songBPM;
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
