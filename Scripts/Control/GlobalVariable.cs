using Godot;
using System;
using Godot.Collections;

public class GlobalVariable : Node
{
	public float songOffset = 0;

	public float interval = 0.15f;

	public string fromScene;

	//Fight Scene Variables

	//[Export]
	//public string player1dir, player2dir;

	//[Export]
	//public string songdatadir;

	//[Export]
	//public bool IsBossfight;

	//Player data
	public SaveData saveData;
	public Stats playerStat;
	public int money;

	public Node currentScene;
	public string currentSceneDir;
	public Camera2D currentCamera;
	public string currentMusic;
	public Humanoid currentPlayer;

	public Resource currentFight;
	public string fightSceneID;

	public bool loadPos = false;

	public SceneTransition sceneTransition;

	//ACTIONS
	[Signal]
	public delegate void Defeated(string name, int ammount);
	public void EmitDefeated(string name)
    {
		EmitSignal("Defeated", name, 1);
    }
	[Signal]
	public delegate void Interacted(string name, int ammount);
	public void EmitInteracted(string name)
    {
		GD.Print("INERACTED WITH " + name);
		EmitSignal("Interacted", name, 1);
		
    }
	[Signal]
	public delegate void GotItem(string name, int ammount);
	public void EmitGotItem(string name, int ammount = 1)
	{
		EmitSignal("GotItem", name, ammount);
	}

	/// <summary>
	/// Used to initialize the fight
	/// </summary>
	/// <param name="player1">String directory of player1</param>
	/// <param name="player2">String directory of player2</param>
	/// <param name="songDatadir">Song data resource</param>
	/// <param name="isBossfight">If is bossfight, no borders. Player can run</param>
	/// 
	/*
	public void InitializeFightData(string player1, string player2, string songData, bool isBossfight, int moneygain, int xpgain)
	{
		SavePlayerPosition();
		player1dir = player1;
		player2dir = player2;
		songdatadir = songData;
		IsBossfight = isBossfight;

		GD.Print(player1dir, player2dir, songdatadir, IsBossfight);
	}
	*/
	public void InitializeFight(string resName)
    {
		SaveGameData();
		FightData fd = ResourceLoader.Load<FightData>("res://Resources/Fights/" + resName + ".tres");
		if(fd!= null)
		{
			fd.Initialize(resName);
			currentFight = fd;
		}
		sceneTransition.ChangeScene("res://Scenes/FightScene.tscn", "STOP");
    }
	public void FinishFight()
    {
		FightData fd = (FightData) currentFight;
		fd.Finished();
		DialogicSharp.Save();
		saveData.money += fd.moneyGain;
		saveData.AddXP(fd.xpGain);
		LoadGameData("FightWon");
    }

	/*
	public void HoldPosition(string sceneDir)
	{
		if (heldPosition.ContainsKey(currentScene.Name)) heldPosition[currentScene.Name] = currentPlayer.GlobalPosition;
		else heldPosition.Add(currentScene.Name, currentPlayer.GlobalPosition);
		DialogicSharp.SetVariable("HeldScene", sceneDir);
		DialogicSharp.SetVariable("HeldMusic", currentMusic);


	}
	*/
	public void OpenMenu(string directory)
	{
		PackedScene ps = ResourceLoader.Load<PackedScene>(directory);
		CanvasLayer instance = ps.Instance<CanvasLayer>();
		GetTree().Root.AddChild(instance);
		if(currentCamera != null)
		{
			//instance.RectGlobalPosition = currentCamera.GetCameraScreenCenter() - instance.RectSize/2;
		}


	}
	public override void _Ready()
	{
		BV.GV = this;
		sceneTransition = GetTree().Root.GetNode<SceneTransition>("SceneTransition");
	}
	public void SaveGameData()
	{
		DialogicSharp.Save();
		saveData.position = currentPlayer.Position;
		saveData.sceneDirectory = currentSceneDir;
		saveData.musicDirectory = currentMusic;

		saveData.stats = playerStat;
		GD.Print("Save Data, position is "+ saveData.position);
		ResourceSaver.Save("user://stat_data.tres", saveData.stats);
		ResourceSaver.Save("user://save_data.tres", saveData);
	}
	public void LoadGameData()
	{
		DialogicSharp.Load();
		saveData = ResourceLoader.Load<SaveData>("user://save_data.tres");
		saveData.stats = ResourceLoader.Load("user://stat_data.tres");
		playerStat = (Stats)saveData.stats;
		GD.Print("Load Data, talisman is " + ((Talisman)((Stats)saveData.stats).talisman).name);
		loadPos = true;
		sceneTransition.ChangeScene(saveData.sceneDirectory, saveData.musicDirectory);

	}
	public void LoadGameData(string dialogue)
	{
		DialogicSharp.Load();
		saveData = ResourceLoader.Load<SaveData>("user://save_data.tres");
		saveData.stats = ResourceLoader.Load("user://stat_data.tres");
		playerStat = (Stats)saveData.stats;
		GD.Print("Load Data, talisman is " + ((Talisman)((Stats)saveData.stats).talisman).name);
		loadPos = true;
		sceneTransition.ChangeScene(saveData.sceneDirectory, saveData.musicDirectory, dialogue);

	}
	public void NewGameData()
	{
		DialogicSharp.ResetSaves();
		saveData = ResourceLoader.Load<SaveData>("res://Resources/DefaultPlayerData.tres");
		playerStat = (Stats)saveData.stats;
		GD.Print("New Data, talisman is "+ ((Talisman)((Stats)saveData.stats).talisman).name);

	}
	public void NewTestGameData()
	{
		DialogicSharp.ResetSaves();
		saveData = ResourceLoader.Load<SaveData>("res://Resources/DebugPlayerData.tres");
		playerStat = (Stats)saveData.stats;

	}

	public void CreateDamageIndicator(int damage, Vector2 Gpos)
	{
		PackedScene DIRes = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/DamageIndicator.tscn");
		DamageIndicator DI = DIRes.Instance<DamageIndicator>();
		DI.damage = damage;
		DI.GlobalPosition = Gpos;
		GetTree().CurrentScene.AddChild(DI);


	}
	public Humanoid FindNpc(string name)
    {
		Humanoid npc = currentScene.GetNodeOrNull<Humanoid>(name);
		return npc;
    }
	public void ExpressNPC(string name, string expression, float duration)
    {
		Humanoid npc = FindNpc(name);
		if(npc == null) { GD.Print("Humanoid "+name+" not found trying to express"); return; }
		Humanoid.Expressions selectedExpression;
        switch (expression)
        {
			case "Angry":
				selectedExpression = Humanoid.Expressions.Angry;break;
			case "Calm":
				selectedExpression = Humanoid.Expressions.Calm; break;
			case "Excited":
				selectedExpression = Humanoid.Expressions.Excited; break;
			case "Happy":
				selectedExpression = Humanoid.Expressions.Happy; break;
			case "Heart":
				selectedExpression = Humanoid.Expressions.Heart; break;
			case "Inspiration":
				selectedExpression = Humanoid.Expressions.Inspiration; break;
			case "Music":
				selectedExpression = Humanoid.Expressions.Music; break;
			case "Poker":
				selectedExpression = Humanoid.Expressions.Poker; break;
			case "Surprised":
				selectedExpression = Humanoid.Expressions.Surprised; break;
			case "Wrong":
				selectedExpression = Humanoid.Expressions.Wrong; break;
			default: GD.Print("Expression " + expression+ " Not Found!"); return; break;
		}
		npc.Express(selectedExpression, duration);
    }
	public void FlipNPC(string name)
    {
		Humanoid npc = FindNpc(name);
		if(npc == null) { GD.Print("Humanoid not found trying to flip"); return; }
		npc.facingLeft = !npc.facingLeft;
	}
	public void MoveNPC(string name, int targetX, int targetY, bool relative)
	{
		Vector2 targetPos = new Vector2(targetX, targetY);
		Humanoid npc = FindNpc(name);
		if (npc == null) { GD.Print("Humanoid " + name + " not found trying pathfind to " + targetPos); return; }
		if (relative)
		{
			npc.SetPathFindRelative(targetPos);
		}
		else
		{
			npc.SetPathFind(targetPos);
		}

	}
	public void CreateNotification(Node parent, string text, float duration = 2.0f, Color c = default)
    {
		PackedScene ps = ResourceLoader.Load<PackedScene>("res://Scenes/UI/Notifier.tscn");
		Notifier n = ps.Instance<Notifier>();
		parent.AddChild(n);
		n.Initialize(text, duration, c);
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("debug"))
		{
			PrintStrayNodes();
		}
	}
}
