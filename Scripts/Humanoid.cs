using Godot;
using Godot.Collections;
using System;

public class Humanoid : Interactable
{
	public enum Expressions
	{
		Angry,
		Calm,
		Excited,
		Happy,
		Heart,
		Inspiration,
		Music,
		Poker,
		Surprised,
		Wrong
	}
	Dictionary<Expressions, string> toExpressionString = new Dictionary<Expressions, string>();


	private int maxHealth = 1000;
	private int health = 1000;
	private int maxInspiration = 100;
	private int inspiration = 100;
	private int defense = 0;
	private int strength = 10;

	private int healthRegenRate = 10;
	private int inspirationRegenRate = 5;
	private int inspirationBlunderRate = 15;

	public Array<SkillForm> skillForms = new Array<SkillForm>();
	public int selectedSkill = 0;
	public AnimationTree animationTree;
	public AnimationNodeStateMachinePlayback stateMachine;

	RayCast2D raycast;
	Timer timer;
	public float AnimSpeed = 0.3f;
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
	public bool isPlayer = false;

	[Export]
	public Array<int> beatsToMove = new Array<int>();
	[Export]
	public bool atkMode;
	[Export]
	public Resource statsResource;
	[Export]
	public bool faceLeft = false;

	public bool facingLeft = false;

	public override void _Ready()
	{
		if(statsResource != null && statsResource is Stats)
		{
			Stats stat = (Stats)statsResource;
			stat.Update();
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
		facingLeft = faceLeft;

		Connect("Blundered", this, "DecreaseInspiration");


		toExpressionString.Add(Expressions.Angry, "Angry");
		toExpressionString.Add(Expressions.Calm, "Calm");
		toExpressionString.Add(Expressions.Excited, "Excited");
		toExpressionString.Add(Expressions.Happy, "Happy");
		toExpressionString.Add(Expressions.Heart, "Heart");
		toExpressionString.Add(Expressions.Inspiration, "Inspiration");
		toExpressionString.Add(Expressions.Music, "Music");
		toExpressionString.Add(Expressions.Poker, "Poker");
		toExpressionString.Add(Expressions.Surprised, "Surprised");
		toExpressionString.Add(Expressions.Wrong, "Wrong");

	}
	public override void _Process(float delta)
	{
		AdjustFacing();
	}
	public bool Move(Vector2 targetPos)
	{
		if (targetPos == Vector2.Zero) return false;
		if (inspiration == 0) { Express(Expressions.Inspiration); return false; }
		if (targetPos.x < 0) facingLeft = true;
		else if (targetPos.x > 0) facingLeft = false;
		
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
		if (targetPos.x < 0) facingLeft = true;
		else if(targetPos.x>0) facingLeft = false;
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
			if (isPlayer)
			{
				proj.SetCollisionMaskBit(2, true);
				proj.SetCollisionMaskBit(1, false);
				proj.SetCollisionLayerBit(1, true);
			}
			else
			{
				proj.SetCollisionMaskBit(2, false);
				proj.SetCollisionMaskBit(1, true);
				proj.SetCollisionLayerBit(1, false);

			}
			proj.setTarget(this, target, basedmg, strength);
			proj.GlobalPosition = this.GlobalPosition;
			root.GetChild(root.GetChildCount() - 1).AddChild(proj);
		}
	}
	/// <summary>
	/// Skill range from 0-2
	/// </summary>
	/// <param name="Target"></param>
	/// <param name="skill"></param>
	public bool Attack(Vector2 Target, int skill = -1)
	{
		int selectedSkill = skill == -1 ? this.selectedSkill : skill;
		if (selectedSkill > skillForms.Count) return false;
		int projectileCost = skillForms[selectedSkill].cost;

		if(inspiration - projectileCost >= 0)
		{
			stateMachine.Start("Attack");

			int baseDmg = skillForms[selectedSkill].damage;
			if (Target.x < Position.x) facingLeft = true;
			else if (Target.x > Position.x) facingLeft = false;
			SpawnProjectile(Target, skillForms[selectedSkill].projectile, baseDmg);
			inspiration -= projectileCost;
			EmitSignal("InspirationChanged", inspiration, maxInspiration);
			timer.WaitTime = ((float)skillForms[selectedSkill].duration-1)  * 60.0f / GlobalHandler.CurrentMusic.songBPM + 45.0f / GlobalHandler.CurrentMusic.songBPM;
			if (!timer.IsConnected("timeout", this, "RefreshState")) { timer.Connect("timeout", this, "RefreshState"); }
			timer.Start();
			return true;
		}
		else
		{

			Express(Expressions.Inspiration);
			return false;
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
	public void RegenInspiration()
	{
		if (currentState != status.Idle) return;
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
			onBlundered();
			inspiration -= inspirationBlunderRate;
			if (inspiration < 0) inspiration = 0;
			EmitSignal("InspirationChanged", inspiration, maxInspiration);
		}
	}
	public override void OpenDialogue(Node2D castSource)
	{
		if (castSource.Position.x < Position.x) facingLeft = true;
		else facingLeft = false;
		Node dialogueNode = DialogicSharp.Start(dialogue);
		GetTree().Root.AddChild(dialogueNode);
		dialogueNode.Connect("tree_exiting", this, "ResetPosition");
	}

	public void ResetPosition()
	{
		facingLeft = faceLeft;
	}
	public void AdjustFacing()
	{
		if (facingLeft)
		{
			sprite.Scale = new Vector2(-1,1);
		}
		else
		{
			sprite.Scale = new Vector2(1, 1);
		}
	}
	public int GetInspirationCount()
	{
		return inspiration;
	}
	public int GetMaxInspirationCount()
	{
		return maxInspiration;
	}
	public void onBlundered()
	{
		Express(Expressions.Wrong);
		stateMachine.Start("Blunder");
	}

	public void Express(Expressions expression, float duration = 1.0f)
	{
		ExpressionBubble expBubble = sprite.GetNodeOrNull<ExpressionBubble>("ExpressionBubble");
		if(expBubble == null)
		{
			PackedScene bubbleScene = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/ExpressionBubble.tscn");
			Node bubbleNode = bubbleScene.Instance();
			sprite.AddChild(bubbleNode);
			expBubble = (ExpressionBubble)bubbleNode;
			expBubble.Position = new Vector2(0, -16);
		}
		expBubble.Start(toExpressionString[expression], duration);
		
	}


}



