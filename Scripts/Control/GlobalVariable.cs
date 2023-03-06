using Godot;
using System;
using Godot.Collections;

public class GlobalVariable : Node
{
	/// <summary>
	/// Music Offset for fight scenes. The higher the number the higher the delay.
	/// </summary>
	public float songOffset = 0;

	/// <summary>
	/// How many percent of the beat can player input.
	/// <para>
	/// Ex: 0.15f, therefore player can input between -15% and 15% in beats. therefore a 30% input rate in beat.
	/// <br/>A 50% interval lets you input in the whole beat.
	/// </para>
	/// </summary>
	public float interval = 0.15f;

	/// <summary>
	/// String that stores from what scene. Will then be passed to the Position2d Inside of the map
	/// </summary>
	public string fromScene;

	//Fight Scene Variables

	/// <summary>
	/// String of player node path
	/// </summary>
	[Export]
	public string player1dir, player2dir;

	/// <summary>
	/// Initialize the fight song with its bpm etc. To make resource, add resource in project, attach songdata as script.
	/// </summary>
	[Export]
	public string songdatadir;

	/// <summary>
	/// Bossfight condition. If true ("y"), player cant run.
	/// </summary>
	[Export]
	public bool IsBossfight;

	//Player data
	public SaveData saveData;
	public Stats playerStat;
	public int money;

	public Node currentScene;
	public string currentSceneDir;
	public Camera2D currentCamera;
	public string currentMusic;
	public Humanoid currentPlayer;

	public bool loadPos = false;
	

	/// <summary>
	/// Used to initialize the fight
	/// </summary>
	/// <param name="player1">String directory of player1</param>
	/// <param name="player2">String directory of player2</param>
	/// <param name="songDatadir">Song data resource</param>
	/// <param name="isBossfight">If is bossfight, no borders. Player can run</param>
	/// 

	public void InitializeFightData(string player1, string player2, string songData, bool isBossfight, int moneygain, int xpgain)
	{
		SavePlayerPosition();
		player1dir = player1;
		player2dir = player2;
		songdatadir = songData;
		IsBossfight = isBossfight;

		GD.Print(player1dir, player2dir, songdatadir, IsBossfight);
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
		playerStat = new Stats();
		playerStat.maxHealth = 1000;
		playerStat.maxInspiration = 200;
		playerStat.strength = 10;
		playerStat.healthRegen = 10;
		playerStat.inspirationRegen = 20;
		playerStat.defense = 0;

		playerStat.forms = new Array<Resource>();
		Resource form1 = ResourceLoader.Load("res://Resources/AttackForms/Cards.tres");
		playerStat.forms.Add(form1);
		Resource form2 = ResourceLoader.Load("res://Resources/AttackForms/Fist.tres");
		playerStat.forms.Add(form2);
		Resource form3 = ResourceLoader.Load("res://Resources/AttackForms/MoonlitSonata2.tres");
		playerStat.forms.Add(form3);
	}

	public void SavePlayerPosition()
	{
		saveData.position = currentPlayer.Position;
		GD.Print("Saved Pos: " + currentPlayer.Position);
		DialogicSharp.SetVariable("HeldScene", currentSceneDir);
		DialogicSharp.SetVariable("HeldMusic", currentMusic);
	}
	public void SaveGameData()
	{
		SavePlayerPosition();
		DialogicSharp.Save();
		saveData.sceneDirectory = currentSceneDir;
		saveData.musicDirectory = currentMusic;
		ResourceSaver.Save("user://save_data.tres", saveData);
	}
	public void LoadGameData()
	{
		DialogicSharp.Load();
		saveData = ResourceLoader.Load<SaveData>("user://save_data.tres");
		loadPos = true;
	}
	public void NewGameData()
	{
		DialogicSharp.ResetSaves();
		saveData = ResourceLoader.Load<SaveData>("res://Resources/DefaultPlayerData.tres");
	}

	public void CreateDamageIndicator(int damage, Vector2 Gpos)
	{
		PackedScene DIRes = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/DamageIndicator.tscn");
		DamageIndicator DI = DIRes.Instance<DamageIndicator>();
		DI.damage = damage;
		DI.GlobalPosition = Gpos;
		GetTree().CurrentScene.AddChild(DI);


	}
	
}
