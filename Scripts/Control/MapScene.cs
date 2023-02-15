using Godot;
using System;

public class MapScene : Node2D
{
    public void PlacePlayer()
    {
        GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
        if (gv.fromScene == null || gv.fromScene == "") return;

        Position2D targetPosition = GetNodeOrNull<Position2D>(gv.fromScene);
        if(targetPosition != null)
        {
            Humanoid player = GetNode<Humanoid>("Player");
            player.Position = targetPosition.Position;

        }
    }
}
