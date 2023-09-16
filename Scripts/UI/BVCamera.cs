using Godot;
using System;

public class BVCamera : Camera2D
{
	public override async void _Ready()
	{
		GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
		gv.currentCamera = this;
		await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
		SmoothingEnabled = true;
		//LimitSmoothed = true;

	}
	public override void _ExitTree()
	{
		GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
		if(gv.currentCamera == this)
		{
			gv.currentCamera = null;
		}
	}
    public override void _EnterTree()
    {
		SmoothingEnabled = false;
		LimitSmoothed = false;
    }
}
