using Godot;
using System;

public abstract class Projectile : Area2D
{
	public int stunDur = 1;
	public int damage = 50;
	public int strength;
	public int speed;
	public Vector2 initialPos;
	public Vector2 targetpoint;
	public AnimationPlayer animplayer;
	public Humanoid caster;
	CollisionShape2D collisionShape;
	public abstract void Move(int beat);
	public bool collided = false;

	public bool onCollideAutoDel = true;

	[Signal]
	public delegate void Hit(int stunDuration);

	public void setTarget(Humanoid caster, Vector2 targetPos, int baseDmg, int str)
	{
		damage = baseDmg;
		strength = str;
		this.caster = caster;
		targetpoint = targetPos;
		Vector2 initialPosition = caster.GlobalPosition;
		initialPos = initialPosition;
		if (targetPos.x < initialPosition.x)
		{
			Scale = new Vector2(-1,1);
		}
		BV.GM.Connect("EmitBeat", this, "Move");
		Connect("area_entered", this, "onHit");
	}
	public int RandomizeDamage()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		int bonusDmg = rng.RandiRange(0, strength);
		return damage + bonusDmg;
	}
	public async void onHit(Node obj)
	{

		if(obj is Humanoid)
		{
			Humanoid h = (Humanoid)obj;
			int newDmg = RandomizeDamage();
			Connect("Hit", h, "Hit");
			EmitSignal("Hit", stunDur, newDmg);
			Disconnect("Hit", h, "Hit");

			GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
			gv.CreateDamageIndicator(newDmg, ((Node2D)obj).GlobalPosition);
		}
		if(obj is BreakableTiles)
		{
			CollisionShape2D colShape = GetNode<CollisionShape2D>("CollisionShape2D");
			
			BreakableTiles tileset = (BreakableTiles)obj;
			Vector2 cell = tileset.WorldToMap(colShape.GlobalPosition);
			tileset.SetCellv(cell, -1);

		}
		if (onCollideAutoDel)
		{
			if (!collided && animplayer != null)
			{
				animplayer.Play("Collide");
			};
			collided = true;
			if(animplayer != null) await ToSignal(animplayer, "animation_finished");
			this.QueueFree();
		}
	}
	
	public override void _Ready(){
		animplayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
		if (animplayer == null) return;
		animplayer.Play("Default");
		
	}
	
	



}
