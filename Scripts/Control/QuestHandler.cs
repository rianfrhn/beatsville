using Godot;
using System;
using Godot.Collections;

public class QuestHandler : Node
{
	public Array<Quest> quests;
	public Array<Quest> activatedQuests;
	public Array<Quest> completedQuests;

	public Node questNode;
	public Node questNodeDefault;
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
		questNodeDefault = questNode.Duplicate();
		
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
		Node parentNode = questNode == null? GetNodeOrNull("Quests") : questNode;
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
		Node parentNode = activatedNode == null? GetNodeOrNull("Activated"): activatedNode;
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
		Node parentNode = completedNode == null? GetNodeOrNull("Completed"): completedNode;
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
	public void ResetQuests()
	{
		questNode.QueueFree();
		foreach(Node n in activatedNode.GetChildren())
		{
			n.QueueFree();
		}
		foreach (Node n in completedNode.GetChildren())
		{
			n.QueueFree();
		}
		Node rollbackNode = questNodeDefault.Duplicate();
		questNode = rollbackNode;
		AddChild(rollbackNode);
		UpdateQuests();

	}

	public void SaveQuests()
	{
		/*
		PackedScene psQuests = new PackedScene();
		psQuests.Pack(questNode);
		ResourceSaver.Save("user://unfQ", psQuests);
		*/
		foreach(Node q in activatedNode.GetChildren())
		{
			foreach(Node n in q.GetChildren())
			{
				n.Owner = activatedNode;
			}
			q.Owner = activatedNode;
			
		}
		foreach (Node q in completedNode.GetChildren())
		{
			foreach (Node n in q.GetChildren())
			{
				n.Owner = completedNode;
			}
			q.Owner = completedNode;
		}
		PackedScene psCompleted = new PackedScene();
		psCompleted.Pack(completedNode);
		
		ResourceSaver.Save("user://compQ.tscn", psCompleted, ResourceSaver.SaverFlags.ReplaceSubresourcePaths);
		

		PackedScene psActivated = new PackedScene();
		psActivated.Pack(activatedNode);
		ResourceSaver.Save("user://actQ.tscn", psActivated, ResourceSaver.SaverFlags.ReplaceSubresourcePaths);



	}
	public void LoadQuests()
	{
		PackedScene psCompleted = ResourceLoader.Load<PackedScene>("user://compQ.tscn", noCache:true);
		completedNode.QueueFree();
		Node comp = psCompleted.Instance();


		AddChild(comp);
		completedNode = comp;

		PackedScene psActivated = ResourceLoader.Load<PackedScene>("user://actQ.tscn", noCache: true);
		activatedNode.QueueFree();
		Node actv = psActivated.Instance();
		AddChild(actv);
		activatedNode = actv;
		ValidateQuests();
		foreach(Node n in actv.GetChildren())
		{
			if(n is Quest q)
			{
				q.Initialize();
				q.Connect("Completed", this, "CompleteQuest");

			}
		}
		UpdateQuests();
	}
	public void ValidateQuests()
	{
		foreach(Node n in activatedNode.GetChildren())
		{
			if (n is Quest q)
			{
				Node duplicate = CheckInQuests(q);
				if (duplicate != null)
				{
					questNode.RemoveChild(duplicate);
					//activatedNode.AddChild(q);
				}
			}
		}
		foreach (Node n in completedNode.GetChildren())
		{
			if (n is Quest q)
			{
				Node duplicate = CheckInQuests(q);
				if (duplicate != null)
				{
					questNode.RemoveChild(duplicate);
					//completedNode.AddChild(q);
				}
			}
		}
	}
	public Node CheckInQuests(Quest quest)
	{
		foreach (Node n in questNode.GetChildren())
		{
			if (n is Quest q)
			{
				if(q.questID == quest.questID)
				{
  					return n;
				}
			}
		}
		return null;
	}

}
