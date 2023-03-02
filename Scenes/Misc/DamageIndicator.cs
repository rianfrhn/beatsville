using Godot;
using System;

public class DamageIndicator : Node2D
{
	public int damage;
	public bool damageMode;
	public override void _Ready()
	{
		RichTextLabel text = GetNode<RichTextLabel>("RichTextLabel");
		Timer timer = GetNode<Timer>("Timer");
		//if(damageMode = true)
		text.BbcodeText = "[center][color=#ff0000][tornado radius=2 freq=5][shake rate=30 level=5]" + damage;
		timer.Connect("timeout", this, "Destroy");
	}
	public void Destroy()
	{
		QueueFree();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
