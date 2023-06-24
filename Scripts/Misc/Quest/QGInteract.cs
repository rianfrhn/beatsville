using System;
using Godot;

public class QGInteract : QuestGoal{
    [Export]
    string interactableName;


    public override void Activate()
    {
        Initialize("Interacted");
    }
    public override void Progress(string name, int ammount)
    {
        if(name == interactableName)
        {
            GD.Print("Goal " + goalID + " Progressed");
            EmitSignal("Progressed");
            Finish();
        }
    }
}
