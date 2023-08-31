using Godot;
using System;
using Godot.Collections;

public class WorldManipulator : Node
{

	public bool canMove = true; //Pas dialogic event, ini buat nentuin bisa move apa kg
	public override void _Ready()
	{
		BV.WM = this;
	}
	public void CreateDamageIndicator(int damage, Vector2 Gpos)
	{
		PackedScene DIRes = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/DamageIndicator.tscn");
		DamageIndicator DI = DIRes.Instance<DamageIndicator>();
		DI.damage = damage;
		DI.GlobalPosition = Gpos;
		GetTree().CurrentScene.AddChild(DI);


	}
	public Humanoid FindNpc(string name)
	{
		Humanoid npc = BV.GH.currentScene.GetNodeOrNull<Humanoid>(name);
		return npc;
	}
	public void ExpressNPC(string name, string expression, float duration)
	{
		Humanoid npc = FindNpc(name);
		if (npc == null) { GD.Print("Humanoid " + name + " not found trying to express"); return; }
		Humanoid.Expressions selectedExpression;
		switch (expression)
		{
			case "Angry":
				selectedExpression = Humanoid.Expressions.Angry; break;
			case "Calm":
				selectedExpression = Humanoid.Expressions.Calm; break;
			case "Excited":
				selectedExpression = Humanoid.Expressions.Excited; break;
			case "Happy":
				selectedExpression = Humanoid.Expressions.Happy; break;
			case "Heart":
				selectedExpression = Humanoid.Expressions.Heart; break;
			case "Inspiration":
				selectedExpression = Humanoid.Expressions.Inspiration; break;
			case "Music":
				selectedExpression = Humanoid.Expressions.Music; break;
			case "Poker":
				selectedExpression = Humanoid.Expressions.Poker; break;
			case "Surprised":
				selectedExpression = Humanoid.Expressions.Surprised; break;
			case "Wrong":
				selectedExpression = Humanoid.Expressions.Wrong; break;
			default: GD.Print("Expression " + expression + " Not Found!"); return;
		}
		npc.Express(selectedExpression, duration);
	}
	public void FlipNPC(string name)
	{
		Humanoid npc = FindNpc(name);
		if (npc == null) { GD.Print("Humanoid not found trying to flip"); return; }
		npc.facingLeft = !npc.facingLeft;
	}
	public void MoveNPC(string name, int targetX, int targetY, bool relative)
	{
		Vector2 targetPos = new Vector2(targetX, targetY);
		Humanoid npc = FindNpc(name);
		if (npc == null) { GD.Print("Humanoid " + name + " not found trying pathfind to " + targetPos); return; }
		if (relative)
		{
			npc.SetPathFindRelative(targetPos);
		}
		else
		{
			npc.SetPathFind(targetPos);
		}

	}
	SceneTreeTween cameratweener;
	public void MoveCamera(int tX, int tY, bool relative, float duration)
	{
		if (BV.GH.currentCamera == null || BV.GH.currentPlayer == null) return;
		Vector2 t = new Vector2(tX, tY);
		if (!relative)
		{
			t -= BV.GH.currentPlayer.GlobalPosition;
		}
		if (cameratweener != null) cameratweener.Kill();
		cameratweener = GetTree().CreateTween();
		cameratweener.TweenProperty(BV.GH.currentCamera, "position", t, duration);


	}

}
