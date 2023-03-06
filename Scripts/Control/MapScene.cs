using Godot;
using System;

public class MapScene : Node2D
{
    public void PlacePlayer(string directory = "")
    {
        GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
        if (gv.loadPos)
        {
            GD.Print("LoadingPosition, Pos: " + gv.saveData.position);
            Vector2 savedPos = gv.saveData.position;
            Humanoid player = GetNode<Humanoid>("Player");
            player.Position = savedPos;
            gv.loadPos = false;
            return;

        }

        if (gv.fromScene == null || gv.fromScene == "") return;

        Position2D targetPosition = GetNodeOrNull<Position2D>(gv.fromScene);
        if(targetPosition != null)
        {
            Humanoid player = GetNode<Humanoid>("Player");
            player.Position = targetPosition.Position;

        }

    }
}
