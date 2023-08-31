using Godot;
using System;

public class BVCamera : Camera2D
{
	public override void _Ready()
	{
		GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
		gv.currentCamera = this;
		
	}
	public override void _ExitTree()
	{
		GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
		if(gv.currentCamera == this)
		{
			gv.currentCamera = null;
		}
	}
}
