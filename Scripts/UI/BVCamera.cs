using Godot;
using System;

public class BVCamera : Camera2D
{
    public override void _Ready()
    {
        GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
        gv.currentCamera = this;
        
    }
    public override void _ExitTree()
    {
        GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
        if(gv.currentCamera == this)
        {
            gv.currentCamera = null;
        }
    }
}
