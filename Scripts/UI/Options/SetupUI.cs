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
	public void DisplayTalisman()
	{
		GridContainer dataGrid = talismanBox.GetNode<GridContainer>("ItemDesc/GridContainer");
		RichTextLabel nameText = container.GetNode<RichTextLabel>("Talismans/ItemName");
		RichTextLabel descText = talismanBox.GetNode<RichTextLabel>("ItemDesc/Desc");
		nameText.Text = displayedTalisman.name;
		//Portrait
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
}
