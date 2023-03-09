using Godot;
using System;
using Godot.Collections;

[Serializable]
[Tool]
public class Stats : Resource
{
	[Export]
	public int baseMaxHealth;
	public int maxHealth;


	[Export]
	public int baseHealthRegen;
	public int healthRegen;

	[Export]
	public int baseMaxInspiration;
	public int maxInspiration;

	[Export]
	public int baseInspirationRegen;
	public int inspirationRegen;
	[Export]
	public int baseDefense;
	public int defense;
	[Export]
	public int baseStrength;
	public int strength;
	[Export]
	public int baseCompetence;
	public int competence;
	[Export]
	public Resource portrait;

	[Export]
	public Array<Resource> forms = new Array<Resource>(null,null,null);

	[Export]
	public Resource equipment;

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
		xp += addedXP;
		if (xp >= levelThreshold[level])
		{
			GD.Print("added " + addedXP + " xp");
			xp -= levelThreshold[level];
			level++;
		}
	}
	public void Update()
	{
		int exH = 0, exHR = 0, exS = 0, exI = 0, exIR = 0, exD = 0, exC = 0;
		if (talisman != null)
		{
			Talisman t = (Talisman)talisman;
			exH += t.health;
			exHR += t.healthRegen;
			exS += t.strength;
			exI += t.inspiration;
			exIR += t.inspirationRegen;
			exD += t.defense;
			exC += t.competence;

		}


		maxHealth = baseMaxHealth + exH;
		healthRegen = baseHealthRegen + exHR;
		strength = baseStrength + exS;
		maxInspiration = baseMaxInspiration + exI;
		inspirationRegen = baseInspirationRegen + exIR;
		defense = baseDefense + exD;
		competence = baseCompetence + exC;

	}



}
