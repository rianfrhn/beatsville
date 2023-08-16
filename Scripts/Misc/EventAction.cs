using Godot;
using System;

public class EventAction : Node
{
    enum Action
    {
        Appear, Disappear, Destroy
    }
    [Export] Action action;
    [Export] string definition;
    [Export] string value;
    CanvasItem parent;
    public override void _Ready()
    {
        parent = GetParent<CanvasItem>();
        onUpdate();
        BV.GV.Connect("UpdateEvent", this, "onUpdate");

    }
    public void onUpdate()
    {
        if (parent == null) return;
        if(DialogicSharp.GetVariable(definition) == value)
        {
            switch (action)
            {
                case Action.Appear:
                    parent.Visible = true;
                    break;
                case Action.Disappear:
                    parent.Visible = false;
                    break;
                case Action.Destroy:
                    parent.QueueFree();
                    break;
            }
        }
    }
}
