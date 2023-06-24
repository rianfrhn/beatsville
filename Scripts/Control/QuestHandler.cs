using Godot;
using System;
using Godot.Collections;

public class QuestHandler : Node
{
	public Array<Quest> quests;
	public Array<Quest> activatedQuests;
	public Array<Quest> completedQuests;

	public Node questNode;
	public Node activatedNode;
	public Node completedNode;
	[Signal]
	public delegate void QuestActivated(string id);

	[Signal]
	public delegate void QuestFinished(string id);
	public void StartQuest(string id)
	{
		GD.Print("Attempting to start quest " + id);
		Quest q = GetQuestByID(id);
		if (q == null) return;
		if (completedQuests.Contains(q)) return;
		GD.Print("Starting quest " + q.questID);
		activatedQuests.Add(q);
		questNode.RemoveChild(q);
		activatedNode.AddChild(q);
		q.Initialize();
		EmitSignal("QuestActivated", q.questID);
		q.Connect("Completed", this, "CompleteQuest");
		BV.GV.CreateNotification(this, "New Quest!", 2.0f, Colors.Yellow);
	}
	public void CompleteQuest(string id)
	{
		Quest q = GetQuestByID(id);
		if (q == null) return;
		if (completedQuests.Contains(q)) return;
		if (!activatedQuests.Contains(q)) return;
		completedQuests.Add(q);
		activatedQuests.Remove(q);

		activatedNode.RemoveChild(q);
		completedNode.AddChild(q);
		EmitSignal("QuestFinished", q.questID);
		BV.GV.CreateNotification(this, "Quest Completed!", 2.0f, Colors.Yellow);
	}

	public override void _Ready()
	{
		BV.QH = this;
		questNode = GetNodeOrNull<Node>("Quests");
		activatedNode = GetNodeOrNull<Node>("Activated");
		completedNode = GetNodeOrNull<Node>("Completed");
		UpdateQuests();
		//StartQuest("ch1");
	}
	public void UpdateQuests()
	{
		quests = GetQuests();
		activatedQuests = GetActivatedQuests();
		completedQuests = GetCompletedQuests();
	}
	public Array<Quest> GetQuests()
	{
		Godot.Collections.Array nodes;
		Array<Quest> quests = new Array<Quest>();
		Node parentNode = GetNodeOrNull("Quests");
		if (parentNode == null) return quests;
		nodes = parentNode.GetChildren();
		foreach (Node item in nodes)
		{
			if (item is Quest q)
			{
				quests.Add(q);
			}
		}
		return quests;
	}
	public Array<Quest> GetActivatedQuests()
	{
		Godot.Collections.Array nodes;
		Array<Quest> quests = new Array<Quest>();
		Node parentNode = GetNodeOrNull("Activated");
		if (parentNode == null) return quests;
		nodes = parentNode.GetChildren();
		foreach (Node item in nodes)
		{
			if (item is Quest q)
			{
				quests.Add(q);
			}
		}
		return quests;

	}
	public Array<Quest> GetCompletedQuests()
	{
		Godot.Collections.Array nodes;
		Array<Quest> quests = new Array<Quest>();
		Node parentNode = GetNodeOrNull("Completed");
		if (parentNode == null) return quests;
		nodes = parentNode.GetChildren();
		foreach (Node item in nodes)
		{
			if (item is Quest q)
			{
				quests.Add(q);
			}
		}
		return quests;

	}
	public Quest GetQuestByID(string id)
	{
		foreach(Quest q in quests)
		{
			if (q.questID == id)
			{
				return q;
			}
		}
		foreach (Quest q in activatedQuests)
		{
			if (q.questID == id)
			{
				return q;
			}
		}

		foreach (Quest q in completedQuests)
		{
			if (q.questID == id)
			{
				return q;
			}
		}
		GD.PushWarning("QUEST WITH ID " + id + " NOT FOUND");
		return null;
	}

}