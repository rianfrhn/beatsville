using Godot;
using System;

public class Interactable : Area2D
{
	[Export]
	public string dialogue;
	public virtual void OpenDialogue(Node2D castSource)
	{
		if(dialogue != null && dialogue != "")
		{
			Node dialogueNode = DialogicSharp.Start(dialogue);
			GetTree().Root.AddChild(dialogueNode);
		}
	}
}
