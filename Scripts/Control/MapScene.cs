using Godot;
using System;

public class MapScene : Node2D
{
	public void PlacePlayer(string directory = "")
	{
		GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
		Humanoid player = GetNode<Humanoid>("Player");
		if (gv.loadPos)
		{
			Vector2 savedPos = gv.saveData.position;
			player.Position = savedPos;
			GD.Print("LoadingPosition, Pos: " + savedPos+ player.Name);
			gv.loadPos = false;
			return;

		}

		if (gv.fromScene == null || gv.fromScene == "") return;
		Position2D targetPosition = GetNodeOrNull<Position2D>(gv.fromScene);
		GD.Print("Loading from other scene, pos: "+targetPosition.Position);
		if(targetPosition != null)
		{
			player.Position = targetPosition.Position;

		}
		gv.fromScene = null;

	}
}
