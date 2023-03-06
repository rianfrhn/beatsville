using Godot;
using System;
using Godot.Collections;

[Serializable]
[Tool]
public class SaveData : Resource
{
	//Stats
	[Export]
	public Resource stats;
	
	//XP
	[Export]
	public int xp;
	
	//Level
	[Export]
	public int level;
	
	//Money
	[Export]
	public int money;
	
	//Talisman (Owned)
	[Export]
	public Array<Resource> talismans;
	
	
	//Talisman Equipped
	[Export]
	public int talismanEquippedId;
	
	
	
	//Scripture(Owned)
	[Export]
	public Array<Resource> scriptures;
	
	
	//Scripture Equipped
	[Export]
	public int scriptureEquippedId;
	
	
	//SkillForms (Owned)
	[Export]
	public Array<Resource> skillForms;

	//SkillForms Equipped
	[Export] public int skill1EquippedId;
	[Export] public int skill2EquippedId;
	[Export] public int skill3EquippedId;
	
	
	//Quests Array
	[Export]
	public Array<Resource> quests;


	//Position
	[Export]
	public string sceneDirectory;
	[Export]
	public string musicDirectory;
	[Export]
	public Vector2 position;
	
	
	
}
