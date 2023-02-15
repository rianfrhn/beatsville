using Godot;
using Godot.Collections;
using System;

public class Humanoid : Interactable
{
	private int maxHealth = 1000;
	private int health = 1000;
	private int maxInspiration = 100;
	private int inspiration = 100;

	private int healthRegenRate = 10;
	private int inspirationRegenRate = 5;
	private int inspirationBlunderRate = 15;

	RayCast2D raycast;
	Timer timer;
	float AnimSpeed = 0.3f;

	[Signal]
	public delegate void HealthChanged(int health, int maxHealth);
	[Signal]
	public delegate void HealthUpdated(int maxHealth);
	[Signal]
	public delegate void InspirationChanged(int inspiration, int maxInspiration);
	[Signal]
	public delegate void InspirationUpdated(int maxInspiration);
	[Signal]
	public delegate void Blundered();
	

	public enum status{Idle, Moving, Attacking, Stunned, Interacting}
	public status currentState;

	

	[Export]
	public Array<int> beatsToMove = new Array<int>();
	[Export]
	PackedScene Projectile;
	private int projectileCost = 10;
	[Export]
	public bool atkMode;

	public override void _Ready()
	{
		raycast = this.GetNode<RayCast2D>("RayCast2D");
		timer = this.GetNode<Timer>("Timer");
		currentState = status.Idle;
		AnimSpeed = GlobalHandler.CurrentMusic != null ? 48.0f/(GlobalHandler.CurrentMusic.songBPM): AnimSpeed;


		Connect("Blundered", this, "DecreaseInspiration");

	}
	public bool Move(Vector2 targetPos)
	{
		if (inspiration == 0) return false;
		raycast.CastTo = targetPos;
		raycast.ForceRaycastUpdate();
		if (!raycast.IsColliding() && currentState == status.Idle)
		{
			currentState = status.Moving;
			SceneTreeTween tweening = GetTree().Root.CreateTween();
			tweening.TweenProperty(this, "position", Position + targetPos, AnimSpeed).SetTrans(Tween.TransitionType.Linear);
			tweening.TweenCallback(this, "RefreshState");
			return true;
		}
		return false;


	}
	public void CheckInteract(Vector2 targetPos)
	{
		raycast.CastTo = targetPos;
		raycast.ForceRaycastUpdate();
		if (raycast.IsColliding()) {
			Interact((Node)raycast.GetCollider());
		}
	}
	public void Interact(Node obj)
	{
		if (obj is Interactable) 
		{
			Interactable o = (Interactable)obj;
			o.OpenDialogue();
		}
	}
	public void RefreshState()
	{
		currentState = status.Idle;
		if (timer.IsConnected("timeout", this, "RefreshState")) { timer.Disconnect("timeout", this, "RefreshState"); }
	}
	void SpawnProjectile( Vector2 target)
	{
		if(currentState == status.Idle)
		{
			Viewport root = GetTree().Root;
			var proj = (Projectile)Projectile.Instance();
			proj.setTarget(this.GlobalPosition, target);
			proj.GlobalPosition = this.GlobalPosition;

			root.GetChild(root.GetChildCount() - 1).AddChild(proj);
			timer.WaitTime = 50 / GlobalHandler.CurrentMusic.songBPM;
			if (!timer.IsConnected("timeout", this, "RefreshState")) { timer.Connect("timeout", this, "RefreshState"); }
			timer.Start();
		}
	}
	public void Attack(Vector2 Target)
	{
		if(inspiration - projectileCost >= 0)
		{
			SpawnProjectile(Target);
			inspiration -= projectileCost;
			EmitSignal("InspirationChanged", inspiration, maxInspiration);
		}
		
	}

	private void Hit(int stundur, int dmg)
	{
		if (health - dmg > 0) health -= dmg;
		else health = 0;
		EmitSignal("HealthChanged", health, maxHealth);
		GD.Print("got hit for " + dmg + " dmg, health is now " + health);
		currentState = status.Stunned;
		timer.WaitTime = stundur * 55 / GlobalHandler.CurrentMusic.songBPM;
		if (!timer.IsConnected("timeout", this, "RefreshState")) { timer.Connect("timeout", this, "RefreshState"); }
		timer.Start();



	}
	private void RegenInspiration()
	{
		if(inspiration < maxInspiration)
		{
			inspiration += inspirationRegenRate;
			if (inspiration > maxInspiration) inspiration = maxInspiration;
			EmitSignal("InspirationChanged", inspiration, maxInspiration);
		}
		else if(health < maxHealth)
		{
			health += healthRegenRate;
			if (health > maxHealth) health = maxHealth;
			EmitSignal("HealthChanged", health, maxHealth);

		}

	}
	private void DecreaseInspiration()
	{
		if (inspiration > 0)
		{
			inspiration -= inspirationBlunderRate;
			if (inspiration < 0) inspiration = 0;
			EmitSignal("InspirationChanged", inspiration, maxInspiration);
		}
	}
	

}


