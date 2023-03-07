using Godot;
using System;
using Godot.Collections;

[Serializable]
[Tool]
public class Stats : Resource
{
	[Export]
	public int maxHealth;
	[Export]
	public int healthRegen;
	[Export]
	public int maxInspiration;
	[Export]
	public int inspirationRegen;
	[Export]
	public int defense;
	[Export]
	public int strength;
	[Export]
	public int competence;
	[Export]
	public Resource portrait;

	[Export]
	public Array<Resource> forms = new Array<Resource>(null,null,null);

	[Export]
	public Resource weapon;

	[Export]
	public Resource talisman;

	//XP
	[Export]
	public int xp;

	//Level
	[Export]
	public int level;

	//Level Thresholds
	[Export]
	public Dictionary<int, int> levelThreshold;

	public void AddXP(int addedXP)
	{
		if (xp + addedXP >= levelThreshold[level])
		{
			xp += addedXP;
			xp -= levelThreshold[level];
			level++;
		}
	}
}
