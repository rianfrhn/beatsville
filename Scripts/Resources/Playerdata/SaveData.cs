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



	public void AddXP(int addedXP)
	{
		if(stats is Stats stat)
		{
			stat.AddXP(addedXP);
		}
	}
	/// <summary>
	/// SLOT starts from 0
	/// </summary>
	/// <param name="slot"></param>
	/// <param name="id"></param>
	public void EquipSkill(int slot, int id)
	{
		foreach(Resource skillRes in skillForms)
		{
			SkillForm skill = (SkillForm)skillRes;
			if(skill.id == id)
			{
				((Stats)stats).forms[slot] = skillRes;
			}
		}
	}



}
