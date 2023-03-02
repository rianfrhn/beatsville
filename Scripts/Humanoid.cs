using Godot;
using Godot.Collections;
using System;

public class Humanoid : Interactable
{
	private int maxHealth = 1000;
	private int health = 1000;
	private int maxInspiration = 100;
	private int inspiration = 100;
	private int defense = 0;
	private int strength = 10;

	private int healthRegenRate = 10;
	private int inspirationRegenRate = 5;
	private int inspirationBlunderRate = 15;

	private Array<SkillForm> skillForms = new Array<SkillForm>();
	public int selectedSkill = 0;
	public AnimationTree animationTree;
	public AnimationNodeStateMachinePlayback stateMachine;

	RayCast2D raycast;
	Timer timer;
	float AnimSpeed = 0.3f;
	Sprite sprite;

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
	public bool atkMode;
	[Export]
	public Resource statsResource;
	[Export]
	public bool faceLeft = false;

	public override void _Ready()
	{
		if(statsResource != null && statsResource is Stats)
		{
			Stats stat = (Stats)statsResource;
			maxHealth = stat.maxHealth;
			health = maxHealth;
			maxInspiration = stat.maxInspiration;
			inspiration = maxInspiration;
			defense = stat.defense;
			healthRegenRate = stat.healthRegen;
			inspirationRegenRate = stat.healthRegen;
			strength = stat.strength;
			foreach(Resource res in stat.forms)
			{
				if (res is SkillForm skill)
				skillForms.Add(skill);
			}

		}
		sprite = GetNodeOrNull<Sprite>("Sprite");
		animationTree = GetNodeOrNull<AnimationTree>("AnimationTree");
		if(animationTree != null)
		{
			stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		}


		raycast = this.GetNode<RayCast2D>("RayCast2D");
		timer = this.GetNode<Timer>("Timer");
		currentState = status.Idle;
		AnimSpeed = GlobalHandler.CurrentMusic != null ? 48.0f/(GlobalHandler.CurrentMusic.songBPM): AnimSpeed;
		sprite.FlipH = faceLeft;

		Connect("Blundered", this, "DecreaseInspiration");

	}
	public bool Move(Vector2 targetPos)
	{
		if (inspiration == 0) return false;
		if (targetPos.x < 0) sprite.FlipH = true;
		else if (targetPos.x > 0) sprite.FlipH = false;
		
		raycast.CastTo = targetPos;
		raycast.ForceRaycastUpdate();
		if (!raycast.IsColliding() && currentState == status.Idle)
		{
			currentState = status.Moving;
			SceneTreeTween tweening = GetTree().Root.CreateTween();
			tweening.TweenProperty(this, "position", Position + targetPos, AnimSpeed).SetTrans(Tween.TransitionType.Linear);
			tweening.TweenCallback(this, "RefreshState");
			if(animationTree != null)
			{
				if (atkMode)
				{
					stateMachine.Start("Jump");
				}
				else
				{
					stateMachine.Travel("Walk");
				}

			}


			return true;
		}
		return false;

	}
	public void CheckInteract(Vector2 targetPos)
	{
		if (targetPos.x < 0) sprite.FlipH = true;
		else if(targetPos.x>0) sprite.FlipH = false;
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
			o.OpenDialogue((Node2D)this);
		}
	}
	public void RefreshState()
	{
		currentState = status.Idle;
		if (animationTree != null)
		{
			stateMachine.Travel("Idle");
		}
		if (timer.IsConnected("timeout", this, "RefreshState")) { timer.Disconnect("timeout", this, "RefreshState"); }
	}
	void SpawnProjectile(Vector2 target, PackedScene projectileScene, int basedmg)
	{

		if (currentState == status.Idle)
		{
			currentState = status.Attacking;
			Viewport root = GetTree().Root;
			Projectile proj = (Projectile)projectileScene.Instance();
			proj.setTarget(this.GlobalPosition, target, basedmg, strength);
			proj.GlobalPosition = this.GlobalPosition;
			root.GetChild(root.GetChildCount() - 1).AddChild(proj);
		}
	}
	public void Attack(Vector2 Target, int skill = -1)
	{
		int selectedSkill = skill == -1 ? this.selectedSkill : skill;
		if (selectedSkill > skillForms.Count) return;
		int projectileCost = skillForms[selectedSkill].cost;

		if(inspiration - projectileCost >= 0)
		{
			stateMachine.Start("Attack");

			int baseDmg = skillForms[selectedSkill].damage;
			if (Target.x < Position.x) sprite.FlipH = true;
			else if (Target.x > Position.x) sprite.FlipH = false;
			SpawnProjectile(Target, skillForms[selectedSkill].projectile, baseDmg);
			inspiration -= projectileCost;
			EmitSignal("InspirationChanged", inspiration, maxInspiration);
			timer.WaitTime = /*skillForms[selectedSkill].duration * */30 / GlobalHandler.CurrentMusic.songBPM;
			if (!timer.IsConnected("timeout", this, "RefreshState")) { timer.Connect("timeout", this, "RefreshState"); }
			timer.Start();
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
			stateMachine.Start("Blunder");
			inspiration -= inspirationBlunderRate;
			if (inspiration < 0) inspiration = 0;
			EmitSignal("InspirationChanged", inspiration, maxInspiration);
		}
	}
	public override void OpenDialogue(Node2D castSource)
	{
		if (castSource.Position.x < Position.x) sprite.FlipH = true;
		else sprite.FlipH = false;
		Node dialogueNode = DialogicSharp.Start(dialogue);
		GetTree().Root.AddChild(dialogueNode);
		dialogueNode.Connect("tree_exiting", this, "ResetPosition");
	}

	public void ResetPosition()
	{
		sprite.FlipH = faceLeft;
	}


}


