using Godot;
using System;
using Godot.Collections;

public class SetupUI : VBoxContainer
{
	GlobalVariable gv;
	SaveData saveData;
	Stats playerStat;
	Control container;
	Control talismanBox;
	Talisman displayedTalisman;
	int talismanArrSize;
	int talismanArrIndex = 0;
	Godot.Collections.Array skillNodeArray = new Godot.Collections.Array();
	Array<Resource> displayedSkillArray = new Array<Resource>();
	Array<int> skillArrIndex = new Array<int>(0, 0, 0);
	int skillArrSize;
	public override void _Ready()
	{
		gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
		saveData = gv.saveData;
		playerStat = (Stats)saveData.stats;
		container = GetNode<Control>("ScrollContainer/VBoxContainer");
		talismanBox = container.GetNode<Control>("Talismans/VBoxContainer");
		displayedTalisman = (Talisman)playerStat.talisman;
		DisplayTalisman();
		talismanArrIndex = GetIndexByTalisman(displayedTalisman);

		Godot.Collections.Array arr = container.GetNode("Skills").GetChildren();
		foreach(object o in arr)
		{
			if (o is HBoxContainer h)
				skillNodeArray.Add(h);
		}
		displayedSkillArray = playerStat.forms;

		for (int i = 0; i < skillNodeArray.Count; i++)
		{
			Control skillNode = (Control)skillNodeArray[i];
			skillArrIndex[i] = GetIndexBySkill(displayedSkillArray[i]);
			Button leftBtn = skillNode.GetNode<Button>("Item/HBoxContainer/Left");
			Button rightBtn = skillNode.GetNode<Button>("Item/HBoxContainer/Right");
			leftBtn.Connect("pressed", this, "onSkillLeftClicked", new Godot.Collections.Array(i));
			rightBtn.Connect("pressed", this, "onSkillRightClicked", new Godot.Collections.Array(i));
		}
		DisplaySkills();

	}
	public void ScrollTalisman(bool ascending = true)
	{
		int dir = ascending ? 1 : -1;
		talismanArrSize = saveData.talismans.Count;
		if (talismanArrIndex + dir >=0 && talismanArrIndex+dir < talismanArrSize)
		{
			talismanArrIndex += dir;
		}
		else
		{
			talismanArrIndex -= (talismanArrSize-1)*dir;
		}
		displayedTalisman = (Talisman)saveData.talismans[talismanArrIndex];
		playerStat.talisman = displayedTalisman;
		gv.playerStat = playerStat;
		talismanArrIndex = GetIndexByTalisman(displayedTalisman);
	}
	public void ScrollSkill(int slot, bool ascending = true)
	{

		int dir = ascending ? 1 : -1;
		skillArrSize = saveData.skillForms.Count;
		if (skillArrIndex[slot] + dir >= 0 && skillArrIndex[slot] + dir < skillArrSize)
		{
			skillArrIndex[slot] += dir;
		}
		else
		{
			skillArrIndex[slot] -= (skillArrSize - 1) * dir;
		}
		displayedSkillArray[slot] = saveData.skillForms[skillArrIndex[slot]];
		playerStat.forms[slot] = displayedSkillArray[slot];
		gv.playerStat = playerStat;
		skillArrIndex[slot] = GetIndexBySkill(displayedSkillArray[slot]);
	}
	public int GetIndexByTalisman(Talisman talisman)
	{
		int i = 0;
		foreach(Resource r in saveData.talismans)
		{
			Talisman t = (Talisman)r;
			if (t == talisman) break;
			i++;
		}
		return i;

	}
	public int GetIndexBySkill(Resource skill)
	{
		int i = 0;
		foreach (Resource r in saveData.skillForms)
		{
			if (r == skill) break;
			i++;
		}
		return i;

	}
	public void DisplayTalisman()
	{
		GridContainer dataGrid = talismanBox.GetNode<GridContainer>("ItemDesc/GridContainer");
		RichTextLabel nameText = container.GetNode<RichTextLabel>("Talismans/ItemName");
		RichTextLabel descText = talismanBox.GetNode<RichTextLabel>("ItemDesc/Desc");
		TextureRect texture = container.GetNode<TextureRect>("Talismans/VBoxContainer/Item/HBoxContainer/CenterContainer/ItemIcon");
		nameText.Text = displayedTalisman.name;
		texture.Texture = displayedTalisman.displayPic;
		descText.Text = displayedTalisman.description;

		RichTextLabel healthText = dataGrid.GetNode<RichTextLabel>("HealthLabel");
		BindData(healthText, displayedTalisman.health, " [color=#1ebc73]Health");

		RichTextLabel hregenText = dataGrid.GetNode<RichTextLabel>("HRegenLabel");
		BindData(hregenText, displayedTalisman.healthRegen, " [color=#91db69]H. Regen");

		RichTextLabel inspirationText = dataGrid.GetNode<RichTextLabel>("InspirationLabel");
		BindData(inspirationText, displayedTalisman.inspiration, " [color=#f79617]Insp");

		RichTextLabel iregenText = dataGrid.GetNode<RichTextLabel>("IRegenLabel");
		BindData(iregenText, displayedTalisman.inspirationRegen, " [color=#f9c22b]I. Regen");

		RichTextLabel strengthText = dataGrid.GetNode<RichTextLabel>("StrengthLabel");
		BindData(strengthText, displayedTalisman.strength, " [color=#ea4f36]Strength");

		RichTextLabel defenseText = dataGrid.GetNode<RichTextLabel>("DefenseLabel");
		BindData(defenseText, displayedTalisman.defense, " [color=#4d9be6]Defense");

		RichTextLabel competenceText = dataGrid.GetNode<RichTextLabel>("CompetenceLabel");
		BindData(competenceText, displayedTalisman.competence, " [color=#905ea9]Comp");

	}
	
	public void DisplaySkills()
	{
		for(int i=0; i<skillNodeArray.Count; i++)
		{
			Control c = (Control)skillNodeArray[i];
			SkillForm sf = (SkillForm)displayedSkillArray[i];
			RichTextLabel text = c.GetNode<RichTextLabel>("Item/ItemName");
			TextureRect pic = c.GetNode<TextureRect>("Item/HBoxContainer/CenterContainer/ItemIcon");
			RichTextLabel desc = c.GetNode<RichTextLabel>("Item/HBoxContainer/Desc");

			if(sf != null)
			{
				text.Text = sf.name;
				pic.Texture = sf.displayPic;
				desc.Text = sf.desc + "\nDamage: " + sf.damage + "\nCost: " + sf.cost;
			}
			else
			{
				text.Text = "Nothing";
				pic.Texture = null;
				desc.Text = "You equipped nothing";
			}

		}


	}
	void BindData(RichTextLabel textLabel, int data, string suffix)
	{
		string prefix = data < 0 ? "" : "+";
		textLabel.Visible = data != 0;
		textLabel.BbcodeText = prefix + data + suffix;
	}
	public void onTalismanLeftClicked()
	{
		ScrollTalisman(false);
		DisplayTalisman();
	}
	public void onTalismanRightClicked()
	{
		ScrollTalisman(true);
		DisplayTalisman();
	}
	public void onSkillLeftClicked(int slot)
	{
		ScrollSkill(slot,false);
		DisplaySkills();
	}
	public void onSkillRightClicked(int slot)
	{
		ScrollSkill(slot,true);
		DisplaySkills();
	}
}
