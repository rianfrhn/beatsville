using Godot;
using System;
using Godot.Collections;

public class ExpressionBubble : Sprite
{
    AnimationPlayer animplayer;
    public override void _Ready()
    {
        animplayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
    }
    public void Start(string anim, float duration)
    {
        animplayer.Stop();
        animplayer.PlaybackSpeed = 3.0f / duration;
        animplayer.Play(anim);
    }
}
