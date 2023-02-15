using Godot;
using System;

public class Interactable : KinematicBody2D
{
    [Export]
    public string dialogue;
    public void OpenDialogue()
    {
        GetTree().Root.AddChild(DialogicSharp.Start(dialogue));
    }
}
