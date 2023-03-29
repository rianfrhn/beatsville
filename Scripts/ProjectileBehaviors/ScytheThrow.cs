using Godot;
using System;

public class ScytheThrow : Projectile
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	bool cd = false;
	int beat = 0;
	Sprite weaponSprite;
	Sprite thisSprite;
	CollisionShape2D collisionShape;
	public override void _Ready()
	{
		onCollideAutoDel = false;
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		Timer cdTimer = GetNode<Timer>("Timer");
		cdTimer.Connect("timeout", this, "onTimeout");
		thisSprite = GetNode<Sprite>("Sprite");
		weaponSprite = caster.GetNodeOrNull<Sprite>("Sprite/Weapon");
		if(weaponSprite != null)
		{
			weaponSprite.Visible = false;
			thisSprite.Texture = weaponSprite.Texture;

		}
	}
	public override void _Process(float delta)
	{
		Position = caster.Position;
		collisionShape.Position = thisSprite.Position;
		thisSprite.Position += new Vector2(0.5f, 0);
		RotationDegrees += 16;
		thisSprite.RotationDegrees -= 4;
	}

	public override void Move(int beat)
	{
		if (!cd) return;
		this.beat++;
		cd = true;
		if (this.beat == 4) Delete();
	}
	public void Delete()
	{
		weaponSprite.Visible = true;
		QueueFree();
	}

	private void onTimeout()
	{
		cd = true;
	}
}
