using Godot;
using System;

public class SaxTravel1 : Projectile
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	bool cd = false;
	int beat = 0;
	CollisionShape2D collisionShape;
	Timer t;
	Vector2 force;
	Node ring;
	Sprite sprite;

	float _speed = 40.0f;
	float _growthRate = 0.1f;
	bool flipcd = false;
	float xscale;
	public override void _Ready()
	{
		base._Ready();
		force = (targetpoint - initialPos).Normalized();
		t = GetNode<Timer>("Timer");
		ring = GetNode<Node2D>("Ring");
		sprite = ring.GetNode<Sprite>("Sprite");
		t.Connect("timeout", this, "onTimerTimeout");
		GetNode<Particles2D>("Particles2D").Emitting = true;
		xscale = Mathf.Sign(Scale.x);
		Scale = new Vector2(0, 0);
		SceneTreeTween sst = GetTree().CreateTween();
		sst.TweenProperty(this, "scale", new Vector2(1 * xscale , 1), 0.1f);
		sst.TweenProperty(this, "position", Position + force * 16, 0.1f);
		AdjustRotation();
	}
	public override void _Process(float delta)
	{
		Position += force * _speed * delta;
		Scale += new Vector2(1 * xscale, 1) * delta * _growthRate;
	}
	public void AdjustRotation()
	{
		Rotation = force.Angle();
	}

	public override void Move(int beat)
	{
		Sprite newSprite = (Sprite)sprite.Duplicate();
		GetParent().AddChild(newSprite);
		newSprite.GlobalPosition = GlobalPosition;
		newSprite.GlobalRotation = GlobalRotation;
		newSprite.ZIndex = ZIndex - 1;
		newSprite.Scale = Scale;
		
		SceneTreeTween sst = GetTree().CreateTween();
		sst.TweenProperty(newSprite, "scale", new Vector2(0, 0), 1.0f);
		sst.TweenProperty(newSprite, "modulate:a", 0.0f, 1.0f);
		sst.TweenCallback(newSprite, "queue_free");

		animplayer.Play("Pulse");



	}
	public override async void Deletion(Node recentlyHit)
	{
		if (!flipcd)
		{
			flipcd = true;
			if(Math.Abs(force.x) >= Math.Abs(force.y))
			{
				force = new Vector2(force.x * -1, force.y);
			}
			else
			{
				force = new Vector2(force.x, force.y * -1);
			}
			/*
			if(Mathf.Abs(((Node2D)recentlyHit).GlobalPosition.x-GlobalPosition.x) >= Mathf.Abs(((Node2D)recentlyHit).GlobalPosition.y - GlobalPosition.y))
			{
				force = new Vector2(force.x, force.y * -1);
			}
			else
			{
				force = new Vector2(force.x * -1, force.y);
			}
			*/
			/*
			bool layer = ((CollisionObject2D)recentlyHit).GetCollisionLayerBit(2);
			if (layer == true)
			{
				SetCollisionMaskBit(2, true);
				SetCollisionMaskBit(1, false);
				SetCollisionLayerBit(1, true);
			}
			else
			{
				SetCollisionMaskBit(2, false);
				SetCollisionMaskBit(1, true);
				SetCollisionLayerBit(1, false);
			}
			*/
			AdjustRotation();
			await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
			flipcd = false;

		}
		//GD.Print("Collided, force= " +Mathf.Sign(force.x));
		t.Start();
		
	}
	public void onTimerTimeout()
	{
		base.Deletion();
	}
}
