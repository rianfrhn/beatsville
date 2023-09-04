using Godot;
using System;

public class Interactable : Area2D
{
	[Export]
	public string dialogue;
	public string dialogueOverride;
    public virtual void OpenDialogue(Node2D castSource)
	{
		if(dialogue != null && dialogue != "")
		{
			string usedDialogue = dialogue;
			if(dialogueOverride != null) usedDialogue = dialogueOverride;
			Node dialogueNode = DialogicSharp.Start(usedDialogue);
			GetTree().Root.AddChild(dialogueNode);
		}
	}
}
