using Godot;
using System;

public class MapScene : Node2D
{
    public override void _Ready()
    {
		PlacePlayer();
		BV.GH.Connect("UpdateEvent", this, "onUpdate");
	}
	public virtual void onUpdate() { }
    public void PlacePlayer(string directory = "")
	{
		GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
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
		if(targetPosition != null)
		{
			player.Position = targetPosition.Position;

		}
		gv.fromScene = null;

	}
}
