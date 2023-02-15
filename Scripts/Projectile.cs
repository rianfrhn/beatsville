using Godot;
using System;

public abstract class Projectile : Area2D
{
	public int stunDur = 4;
	public int damage = 50;
	public int speed;
	public Vector2 initialPos;
	public Vector2 targetpoint;
	public abstract void Move(int beat);

	[Signal]
	public delegate void Hit(int stunDuration);

	public void setTarget(Vector2 initialPosition, Vector2 targetPos)
	{
		targetpoint = targetPos;
		initialPos = initialPosition;
		GlobalHandler.CurrentMusic.Connect("EmitBeat", this, "Move");
		Connect("body_entered", this, "onHit");
	}
	public void onHit(Node obj)
	{
		if(obj is Humanoid)
		{
			Humanoid h = (Humanoid)obj;
			
			Connect("Hit", h, "Hit");
			EmitSignal("Hit", stunDur, damage);
			Disconnect("Hit", h, "Hit");
		}
		this.QueueFree();
	}
	



}
