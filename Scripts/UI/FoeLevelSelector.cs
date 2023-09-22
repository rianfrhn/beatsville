using Godot;
using System;
using Godot.Collections;

public class FoeLevelSelector : CanvasLayer
{
	Control levelContainer;
	Control fightInfo;
	Control diffSelector;



	Array<FightData> fights;
	FightData selected = null;
	int selectedDifficulty = 0;
	string selectedFoeName = "Shadow";
	SongData prevSong; //Ini buat nyimpen song sblmnya, bakal switch lg pas close
	string prevChosenSong; //Ini buat checking kalo switch song
	public void Close()
	{
		BV.GM.SwitchMusicByResource(prevSong);
		QueueFree();
	}
	public override void _Ready()
	{
		prevSong = BV.GM.songData;
		levelContainer = GetNodeOrNull<Control>("HBoxContainer/Selection/VBoxContainer/ScrollContainer/VBoxContainer");
		fightInfo = GetNodeOrNull<Control>("HBoxContainer/Info/ScrollContainer/FightInfo");
		diffSelector = GetNodeOrNull<Control>("HBoxContainer/Info/NinePatchRect/DiffSelector");
		Button easyBtn = GetNodeOrNull<Button>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/EasyButton");
		Button mediumBtn = GetNodeOrNull<Button>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/MediumButton");
		Button hardBtn = GetNodeOrNull<Button>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/HardButton");
		Button extremeBtn = GetNodeOrNull<Button>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/ExtremeButton");
		Button exitButton = GetNodeOrNull<Button>("Exit");
		exitButton.Connect("pressed", this, "Close");
		GetNode<Button>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/HBoxContainer/StartButton").Connect("pressed", this, "Start");


		if (levelContainer != null)
		{
			foreach(FoeLevelButton n in levelContainer.GetChildren())
			{
				n.Connect("FoeSelected", this, "onFoeSelected");
			}
		}
		easyBtn.Connect("pressed", this, "onDifficultySelected", new Godot.Collections.Array(0));
		mediumBtn.Connect("pressed", this, "onDifficultySelected", new Godot.Collections.Array(1));
		hardBtn.Connect("pressed", this, "onDifficultySelected", new Godot.Collections.Array(2));
		extremeBtn.Connect("pressed", this, "onDifficultySelected", new Godot.Collections.Array(3));
	}
	public void onFoeSelected(string name, FightData easy, FightData normal, FightData hard, FightData extreme)
	{
		selectedFoeName = name;
		fights = new Array<FightData>(easy,normal,hard,extreme);
		UpdateFightInfoScreen();
	}
	public void onDifficultySelected(int diff)
	{
		selectedDifficulty = diff;
		UpdateFightInfoScreen();
	}
	public void UpdateFightInfoScreen()
	{
		diffSelector.Visible = true;
		fightInfo.Visible = true;

		selected = fights[selectedDifficulty];
		TextureRect photo = fightInfo.GetNode<TextureRect>("Data/VBoxContainer/Portrait");
		Button health = fightInfo.GetNode<Button>("Data/VSplitContainer/GridContainer/Health/Label");
		Button hregen = fightInfo.GetNode<Button>("Data/VSplitContainer/GridContainer/HRegen/Label");
		Button insp = fightInfo.GetNode<Button>("Data/VSplitContainer/GridContainer/Inspiration/Label");
		Button iregen = fightInfo.GetNode<Button>("Data/VSplitContainer/GridContainer/IRegen/Label");
		Button strength = fightInfo.GetNode<Button>("Data/VSplitContainer/GridContainer/Strength/Label");
		Button def = fightInfo.GetNode<Button>("Data/VSplitContainer/GridContainer/Defense/Label");
		RichTextLabel songInfo = GetNode<RichTextLabel>("HBoxContainer/Info/ScrollContainer/FightInfo/SongInfo");
		RichTextLabel rewards = GetNode<RichTextLabel>("HBoxContainer/Info/ScrollContainer/FightInfo/Rewards");

		RichTextLabel namelabel = fightInfo.GetNode<RichTextLabel>("RichTextLabel");
		namelabel.BbcodeText = "* " + selectedFoeName;

		
		Humanoid npc = selected.player2.Instance<Humanoid>();
		Stats stats = (Stats)npc.statsResource;
		SongData sd = (SongData)selected.songData;

		health.Text = stats.baseMaxHealth.ToString() + " ";
		hregen.Text = stats.baseHealthRegen.ToString() + " ";
		insp.Text = stats.maxInspiration.ToString() + " ";
		iregen.Text = stats.inspirationRegen.ToString() + " ";
		strength.Text = stats.baseStrength.ToString() + " ";
		def.Text = stats.defense.ToString() + " ";
		if(prevChosenSong != sd.displayName)
		{
			prevChosenSong = sd.displayName;
			BV.GM.SwitchMusicByResource(sd);
		}


		songInfo.BbcodeText = sd.displayName + "\nBPM: " + sd.BPM.ToString() + "\nDuration: " + Mathf.RoundToInt(BV.GM.GetPlayingAudioStream().Stream.GetLength() / 60).ToString() + ":" + Mathf.RoundToInt(BV.GM.GetPlayingAudioStream().Stream.GetLength() % 60).ToString();
		rewards.BbcodeText = "- 10 Bits";

		BVTextButton easyBtn = GetNodeOrNull<BVTextButton>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/EasyButton");
		BVTextButton mediumBtn = GetNodeOrNull<BVTextButton>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/MediumButton");
		BVTextButton hardBtn = GetNodeOrNull<BVTextButton>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/HardButton");
		BVTextButton extremeBtn = GetNodeOrNull<BVTextButton>("HBoxContainer/Info/NinePatchRect/DiffSelector/HBoxContainer/ExtremeButton");
		switch (selectedDifficulty)
		{
			case 0: 
				easyBtn.Text = "*Easy*";
				mediumBtn.Text = "Medium";
				hardBtn.Text = "Hard";
				extremeBtn.Text = "Extreme";
				easyBtn.ChangeDefaultColor(new Color("#5AFF70"));
				mediumBtn.ChangeDefaultColor(new Color(1, 1, 1));
				hardBtn.ChangeDefaultColor(new Color(1, 1, 1));
				extremeBtn.ChangeDefaultColor(new Color(1, 1, 1));
				break;
			case 1:
				easyBtn.Text = "Easy";
				mediumBtn.Text = "*Medium*";
				hardBtn.Text = "Hard";
				extremeBtn.Text = "Extreme";
				easyBtn.ChangeDefaultColor(new Color(1, 1, 1));
				mediumBtn.ChangeDefaultColor(new Color("#91AA4B"));
				hardBtn.ChangeDefaultColor(new Color(1, 1, 1));
				extremeBtn.ChangeDefaultColor(new Color(1, 1, 1));
				break;
			case 2:
				easyBtn.Text = "Easy";
				mediumBtn.Text = "Medium";
				hardBtn.Text = "*Hard*";
				extremeBtn.Text = "Extreme";
				easyBtn.ChangeDefaultColor(new Color(1, 1, 1));
				mediumBtn.ChangeDefaultColor(new Color(1, 1, 1));
				hardBtn.ChangeDefaultColor(new Color("#C85525"));
				extremeBtn.ChangeDefaultColor(new Color(1, 1, 1));
				break;
			case 3:
				easyBtn.Text = "Easy";
				mediumBtn.Text = "Medium";
				hardBtn.Text = "Hard";
				extremeBtn.Text = "*Extreme*";
				easyBtn.ChangeDefaultColor(new Color(1, 1, 1));
				mediumBtn.ChangeDefaultColor(new Color(1, 1, 1));
				hardBtn.ChangeDefaultColor(new Color(1, 1, 1));
				extremeBtn.ChangeDefaultColor(new Color("#FF0000"));
				break;
		}


		npc.QueueFree();

	}
	public void Start()
	{
		BV.GM.FadeOff();
		BV.GH.SaveGameData();
		BV.GH.InitializeFight(selected.ResourcePath);
		QueueFree();
	}
}
