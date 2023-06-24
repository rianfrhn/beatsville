using Godot;
using Godot.Collections;
using System;

public class QuestUI : VBoxContainer
{
    public Dictionary<BVTextButton, Quest> questButtons = new Dictionary<BVTextButton,Quest>();
    VBoxContainer container;
    RichTextLabel label;
    BVTextButton blueprintBtn;
    public override void _Ready()
    {
        container = GetNodeOrNull<VBoxContainer>("ScrollContainer/ItemList");
        label = GetNodeOrNull<RichTextLabel>("Desc");
        Connect("visibility_changed", this, "UpdateQuests");
        GD.Print("AAAAAA");
        Connect("visibility_changed", this, "onVisibilityChanged");
    }
    public void onVisibilityChanged()
    {
        if (!Visible) return;
        questButtons = new Dictionary<BVTextButton, Quest>();
        blueprintBtn = container.GetNode<BVTextButton>("Quest");
        foreach (Node n in container.GetChildren())
        {
            if(n != blueprintBtn)
            {
                n.QueueFree();
            }
        }
        GD.Print("Total of quests: " + BV.QH.activatedQuests.Count);
        foreach(Quest q in BV.QH.activatedQuests)
        {
            GD.Print(q.questID);
            BVTextButton newBtn = (BVTextButton)blueprintBtn.Duplicate();
            container.AddChild(newBtn);
            newBtn.Name = q.questID;
            newBtn.Text = q.questName;
            newBtn.Visible = true;
            newBtn.Connect("pressed", this, "OnPressed", new Godot.Collections.Array(q));
            
            questButtons.Add(newBtn, q);

        }
        label.BbcodeText = "";
    }
    public void OnPressed(Quest q)
    {
        label.BbcodeText = q.questName+": "+q.description+"\nRewards: NotYetImplemented";
    }
}
