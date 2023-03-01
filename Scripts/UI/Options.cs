using Godot;
using System;
using Godot.Collections;

public class Options : CanvasLayer
{
	public Dictionary<string, BVTextButton> Buttons = new Dictionary<string, BVTextButton>();
	public Dictionary<string, Control> TargetFrames = new Dictionary<string, Control>();
	string current;
	public override void _Ready()
	{
		Control xButton = GetNode<Control>("Exit");
		xButton.Connect("pressed", this, "Close");

		Control menus = GetNode("MenuSeparate").GetNode("LeftSide").GetNode("Menus").GetNode("ScrollContainer").GetNode<Control>("VBoxContainer");
		Control panels = GetNode("MenuSeparate").GetNode<Control>("RightSide");
		foreach(BVTextButton button in menus.GetChildren())
		{
			Buttons.Add(button.Name, button);
			TargetFrames.Add(button.Name, panels.GetNodeOrNull<Control>(button.Name));
			button.Connect("pressed", this, "OpenMenu", new Godot.Collections.Array() { button.Name});
		}
	}

	public void OpenMenu(string name)
	{
		if(current != null && TargetFrames[current] != null)
		{
			TargetFrames[current].Visible = false;
		}
		if(TargetFrames[name] != null) TargetFrames[name].Visible = true;
		current = name;
	}
	public void Close()
	{
		QueueFree();
	}
}
