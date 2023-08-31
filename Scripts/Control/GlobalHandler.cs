using Godot;
using System;
using Godot.Collections;

public class GlobalHandler : Node
{
	public float songOffset = 0;

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
	public Humanoid currentAtkPlayer;

	public Resource currentFight;
	public string fightSceneID;

	public bool loadPos = false;

	public SceneTransition sceneTransition;
	[Signal]
	public delegate void UpdateEvent(); //ini buat ngeupdate yang ada di definition dialogic

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
		//SaveGameData();
		FightData fd = ResourceLoader.Load<FightData>(resName);
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
		BV.GH = this;
		sceneTransition = GetTree().Root.GetNode<SceneTransition>("SceneTransition");
	}
	public void SaveGameData()
	{
		BV.QH.SaveQuests();
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
		BV.QH.LoadQuests();
		DialogicSharp.Load();
		saveData = ResourceLoader.Load<SaveData>("user://save_data.tres", noCache: true);
		saveData.stats = ResourceLoader.Load("user://stat_data.tres");
		playerStat = (Stats)saveData.stats;
		GD.Print("Load Data, talisman is " + ((Talisman)((Stats)saveData.stats).talisman).name);
		loadPos = true;
		sceneTransition.ChangeScene(saveData.sceneDirectory, saveData.musicDirectory);

	}
	public void LoadGameData(string dialogue)
	{
		BV.QH.LoadQuests();
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
		BV.QH.ResetQuests();
		DialogicSharp.ResetSaves();
		saveData = ResourceLoader.Load<SaveData>("res://Resources/DefaultPlayerData.tres");
		playerStat = (Stats)saveData.stats;
		GD.Print("New Data, talisman is "+ ((Talisman)((Stats)saveData.stats).talisman).name);

	}
	public void NewTestGameData()
	{
		BV.QH.ResetQuests();
		DialogicSharp.ResetSaves();
		saveData = ResourceLoader.Load<SaveData>("res://Resources/DebugPlayerData.tres");
		playerStat = (Stats)saveData.stats;

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
	public void OnUpdateEvent()
    {
		SaveGameData();
    }
}
