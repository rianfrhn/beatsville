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
	public enum AnimState
	{
		Idle, Moving, Attacking, Stunned
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
	public NavigationAgent2D navigationAgent;
	public AnimationPlayer animPlayer;
	public AnimationPlayer atkAnimPlayer;

	RayCast2D raycast;
	Timer timer;
	public float AnimSpeed = 0.3f;
	//Sprite sprite;
	Node2D body;

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
	[Signal]
	public delegate void Defeated();

	public enum status{Idle, Moving, Attacking, Stunned, Interacting}
	public status currentState;
	private float walkAnimPos = 0.0f;

	//Pathfindings
	private int PathTarget = 0;
	private TileMap worldTileMap;
	private AStarPath AstarPathFind;
	private bool pathing = false;
	[Export]
	public Texture battlePicture = null;
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
	int xFacing = 0;
	SceneTreeTween tweening;//forface
	SceneTreeTween tweening2;//formove

	public int attackCombo = 0;
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
		body = GetNodeOrNull<Node2D>("Rig");
		//sprite = GetNodeOrNull<Sprite>("Sprite");
		worldTileMap = GetParent().GetNodeOrNull<TileMap>("PathTileMap");
		AstarPathFind = GetNodeOrNull<AStarPath>("AStarPath");
		if(AstarPathFind != null && worldTileMap != null) AstarPathFind.SetTileMap(worldTileMap);

		animPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
		atkAnimPlayer = GetNodeOrNull<AnimationPlayer>("AttackAnimationPlayer");
		navigationAgent = GetNodeOrNull<NavigationAgent2D>("NavigationAgent2D");


		raycast = this.GetNode<RayCast2D>("RayCast2D");
		timer = this.GetNode<Timer>("Timer");
		currentState = status.Idle;
		if (atkMode) AnimSpeed = BV.GM != null ? 48.0f/(BV.GM.songBPM): AnimSpeed;
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

		RefreshState();
	}
	public override void _Process(float delta)
	{
		
		if(body != null && !(facingLeft && xFacing == -1) && !(!facingLeft && xFacing == 1))
		AdjustFacing();
		if (pathing && currentState == status.Idle)
		{
			WalkPath();
		}
			


	}
	public void SetPathFind(Vector2 targetPos)
	{
		Vector2 tp = CastToGrid(targetPos);
		if (AstarPathFind.SetAstarPath(GlobalPosition, tp))
		{
			pathing = true;
			PathTarget = 1;
		}
	}
	public void SetPathFindRelative(Vector2 targetPos)
	{
		if (AstarPathFind.SetAstarPath(GlobalPosition, GlobalPosition + targetPos))
		{
			pathing = true;
			PathTarget = 1;
		}
	}
	public bool Move(Vector2 targetPos)
	{
		if (targetPos == Vector2.Zero) return false;
		if (inspiration == 0) { Express(Expressions.Inspiration); return false; }
		
		raycast.CastTo = targetPos;
		raycast.CollideWithAreas = false;
		raycast.ForceRaycastUpdate();
		if (!raycast.IsColliding())
		{
			bool a = GoTo(targetPos);


			return a;
		}
		return false;

	}
	public bool GoTo(Vector2 targetPos, bool relative = true)
	{
		int rel = relative ? 1 : 0;
		Vector2 trueTarget = (Position * rel + targetPos);
		if (trueTarget.x - Position.x < 0) facingLeft = true;
		else if (trueTarget.x - Position.x > 0) facingLeft = false;
		if (currentState != status.Idle) return false;
		float distance = (trueTarget-Position).Length();
		currentState = status.Moving;

		if (tweening2 != null) tweening2.Kill();
		tweening2 = GetTree().CreateTween();
		tweening2.TweenProperty(this, "position", trueTarget, AnimSpeed * distance/ 16).SetTrans(Tween.TransitionType.Linear);
		tweening2.TweenCallback(this, "RefreshState");
		if (animPlayer != null)
		{
			if (atkMode)
			{
				attackCombo = 0;
				if (animPlayer.CurrentAnimation != "Jump")
					animPlayer.Play("Jump");
			}
			else
			{
				if(animPlayer.CurrentAnimation != "Walk")
				{
					animPlayer.PlaybackSpeed = 0.5f / AnimSpeed;
					animPlayer.Play("Walk");
					animPlayer.Seek(walkAnimPos);
				}
			}

		}
		return true;
	}
	public Vector2 CastToGrid(Vector2 pos)
	{
		int x = (int)(pos.x - (pos.x % 16) + 8);
		int y = (int)(pos.y - (pos.y % 16) + 8);
		return new Vector2(x, y);
	}
	public void WalkPath()
	{
		if (PathTarget < AstarPathFind.pathNodeList.Count)
		{
			var targetNode = worldTileMap.MapToWorld(AstarPathFind.pathNodeList[PathTarget]) + AstarPathFind.halfTileSize;
			GoTo(targetNode, false);
			if(Position == targetNode)
			{
				PathTarget++;
			}
		}
		else
		{
			pathing = false;
		}
	}
	public void CheckInteract(Vector2 targetPos)
	{
		if (atkMode || currentState != status.Idle) return;
		if (targetPos.x < 0) facingLeft = true;
		else if(targetPos.x>0) facingLeft = false;
		raycast.CastTo = targetPos;
		raycast.CollideWithAreas = true;
		raycast.ForceRaycastUpdate();
		if (raycast.IsColliding()) {
			Node body = (Node)raycast.GetCollider();
			Interact(body);
		}
	}
	public void Interact(Node obj)
	{
		if (obj is Interactable) 
		{
			Interactable o = (Interactable)obj;
			o.OpenDialogue(this);
			BV.GV.EmitInteracted(o.Name);
		}
	}
	public async void RefreshState()
	{
		if (timer.IsConnected("timeout", this, "RefreshState")) { timer.Disconnect("timeout", this, "RefreshState"); }
		currentState = status.Idle;
		walkAnimPos = animPlayer.CurrentAnimationPosition;
		animPlayer.PlaybackSpeed = 1;

		if (atkAnimPlayer != null && atkAnimPlayer.IsPlaying()) return;

		animPlayer.Play("RESET");
		await ToSignal(animPlayer, "animation_finished");
		animPlayer.Play("Idle");
	}
	void SpawnProjectile(Vector2 target, Projectile proj, int basedmg)
	{

		if (currentState == status.Idle)
		{
			currentState = status.Attacking;
			Viewport root = GetTree().Root;
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

		SkillForm skillForm = skillForms[selectedSkill];
		int projectileCost = skillForm.cost;

		//Check combo
		if (skillForm.projectiles.Count == 0) return false;
		if (attackCombo >= skillForm.projectiles.Count) attackCombo = 0;

		PackedScene projectile = skillForm.projectiles[attackCombo];
		
		if(inspiration - projectileCost >= 0)
		{
			Projectile p = projectile.Instance<Projectile>();
			int baseDmg = skillForm.damage;
			//GD.Print("Target " + Target);
			//GD.Print("Position " + Position);
			if (Target.x < Position.x) { facingLeft = true; }
			else if (Target.x > Position.x) { facingLeft = false; }
			SpawnProjectile(Target, p, baseDmg);
			inspiration -= projectileCost;
			EmitSignal("InspirationChanged", inspiration, maxInspiration);
			timer.WaitTime = 45.0f / BV.GM.songBPM;
			if (!timer.IsConnected("timeout", this, "RefreshState")) { timer.Connect("timeout", this, "RefreshState"); }
			timer.Start();

			if (atkAnimPlayer != null)
			{
				string animationName = "Attack";

				if (p.castingAnimation != null)
				{
					Animation a = p.castingAnimation;
					string skillName = a.ResourceName;
					if (!atkAnimPlayer.HasAnimation(skillName))
					{
						atkAnimPlayer.AddAnimation(skillName, a);
					}
					animationName = skillName;
				}

				atkAnimPlayer.PlaybackSpeed = 1 / (55.0f / BV.GM.songBPM);
				atkAnimPlayer.Seek(0, true);
				atkAnimPlayer.Play(animationName);
			}
			attackCombo++;
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
		animPlayer.Play("Hurt");
		if (health - dmg > 0) health -= dmg;
		else
		{
			health = 0;
		}
		if(health == 0)
		{
			EmitSignal("Defeated");
		}
		EmitSignal("HealthChanged", health, maxHealth);
		GD.Print("got hit for " + dmg + " dmg, health is now " + health);
		currentState = status.Stunned;
		timer.WaitTime = stundur * 55 / BV.GM.songBPM;
		if (!timer.IsConnected("timeout", this, "RefreshState")) { timer.Connect("timeout", this, "RefreshState"); }
		timer.Start();



	}
	public void RegenInspiration()
	{
		if (currentState != status.Idle) return;
		attackCombo = 0;
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
		if (body == null) return;
		if (facingLeft)
		{
			//sprite.Scale = new Vector2(-1,1);
			//body.Scale = new Vector2(-1, 1);
			if (tweening != null) tweening.Kill();
			tweening = GetTree().CreateTween();
			tweening.TweenProperty(body, "scale:x", -1.0f, 0.1f).SetTrans(Tween.TransitionType.Sine);
			xFacing = -1;
		}
		else
		{
			//sprite.Scale = new Vector2(1, 1);
			//body.Scale = new Vector2(1, 1);
			if (tweening != null) tweening.Kill();
			tweening = GetTree().CreateTween();
			tweening.TweenProperty(body, "scale:x", 1.0f, 0.1f).SetTrans(Tween.TransitionType.Sine);
			xFacing = 1;

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
		animPlayer.Play("Blunder");
		attackCombo = 0;
	}

	public void Express(Expressions expression, float duration = 1.0f)
	{
		//TODO: PLS CHANGE THIS TO BODY LATER
		ExpressionBubble expBubble = body.GetNodeOrNull<ExpressionBubble>("ExpressionBubble");
		if(expBubble == null)
		{
			PackedScene bubbleScene = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/ExpressionBubble.tscn");
			Node bubbleNode = bubbleScene.Instance();
			body.AddChild(bubbleNode);
			expBubble = (ExpressionBubble)bubbleNode;
			expBubble.Position = new Vector2(0, -16);
			expBubble.ZIndex = 1;
		}
		expBubble.Start(toExpressionString[expression], duration);
		
	}
	
	public int GetHealth()
	{
		return health;
	}



}



