using Godot;
using System;
using Godot.Collections;

public class Quest : Node
{
	[Export]
	public string questID= "";
	[Export]
	public string questName = "";
	[Export]
	public string description = "";
	[Export]
	public bool showProgress = false;
	[Export]
	public bool sequential = true;
	public Array<QuestGoal> questGoals = new Array<QuestGoal>();
	public int progress;
	[Signal]
	public delegate void Completed(string id);
	public void Initialize()
	{
		questGoals = GetQuestGoals();

		ActivateQueue();
	}
	public void ActivateQueue()
	{
		GD.Print("Attempting to initialize Quest " + questID);
		if (questGoals == null)
		{
			GD.PushWarning("QUEST GOALS FOR " + questID + " NOT FOUND");
			return;
		}
		if(questGoals.Count == 0)
		{
			GD.PushWarning("QUEST GOALS FOR " + questID + " IS 0");
			return;
		}
		progress = 0;
		GD.Print("Quest " + questID +" INITIALIZED");
        if (sequential)
		{
			ActivateGoal(0);
        }
        else
        {
			for (int i = 0; i < questGoals.Count; i++) ActivateGoal(i);
        }
	}
	public void ActivateGoal(int num)
	{
		if (questGoals == null || questGoals[num] == null) return;
		QuestGoal q = questGoals[num];
		GD.Print("Goal " + q.goalID + " activated");
		q.Activate();
		q.Connect("Completed", this, "onGoalCompleted");
	}
	public void onGoalCompleted(string id)
	{
		QuestGoal q = GetQuestGoalByID(id);
		GD.Print("QuestGoal: " + id + " has been completed");
		q.Disconnect("Completed", this, "onGoalCompleted");
		progress++;
		GD.PushWarning("Progress = " + progress);
		GD.PushWarning("Goal Count = " + questGoals.Count);

		if (progress < questGoals.Count)
		{
			if (sequential) ActivateGoal(progress);
			BV.GH.CreateNotification(this, "Quest Progressed!", 2.0f, Colors.Yellow);
		}
		else
		{
			EmitSignal("Completed", questID);
			GD.Print("Quest " + questID + " Completed");
		}
	}
	
	public Array<QuestGoal> GetQuestGoals()
	{
		Godot.Collections.Array nodes;
		Array<QuestGoal> quests = new Array<QuestGoal>();
		nodes = GetChildren();
		foreach (Node item in nodes)
		{
			if (item is QuestGoal q)
			{
				quests.Add(q);
			}
		}
		return quests;
	}
	public QuestGoal GetQuestGoalByID(string id)
	{
		foreach (QuestGoal q in questGoals)
		{
			if (q.goalID == id)
			{
				return q;
			}
		}
		return null;
	}



}
