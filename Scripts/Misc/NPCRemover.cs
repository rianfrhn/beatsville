using Godot;
using System;

public class NPCRemover : Area2D
{

	public override void _Ready()
	{
		Connect("area_entered", this, "onEntityEntered");
		
	}
	private void onEntityEntered(Node body)
	{
		if (!Visible) return;
		if (!(body is Humanoid npc && !npc.isPlayer)) return;
		npc.QueueFree();
	}


}
