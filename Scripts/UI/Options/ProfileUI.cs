using Godot;
using System;

public class ProfileUI : VBoxContainer
{
    Control statsBox;
    GlobalHandler gh;
    Button
        healthTxt, regenHealthTxt,
        inspirationTxt, regenInspirationTxt,
        strengthTxt, defenseTxt, competenceTxt;

    RichTextLabel progressTxt;
    ProgressBar progressBar;

    RichTextLabel desc;

    public override void _Ready()
    {
        gh = BV.GH;
        statsBox = GetNode<GridContainer>("Data/VSplitContainer/GridContainer");
        healthTxt = statsBox.GetNode<Button>("Health/Label");
        regenHealthTxt = statsBox.GetNode<Button>("HRegen/Label");
        inspirationTxt = statsBox.GetNode<Button>("Inspiration/Label");
        regenInspirationTxt = statsBox.GetNode<Button>("IRegen/Label");
        strengthTxt = statsBox.GetNode<Button>("Strength/Label");
        defenseTxt = statsBox.GetNode<Button>("Defense/Label");
        competenceTxt = statsBox.GetNode<Button>("MusicalCompetence/Label");
        desc = GetNode<RichTextLabel>("RichTextLabel");

        healthTxt.Connect("pressed", this, "onHealthPressed");
        regenHealthTxt.Connect("pressed", this, "onRegenHealthPressed");
        inspirationTxt.Connect("pressed", this, "onInspirationPressed");
        regenInspirationTxt.Connect("pressed", this, "onRegenInspirationPressed");
        strengthTxt.Connect("pressed", this, "onStrengthPressed");
        defenseTxt.Connect("pressed", this, "onDefensePressed");
        competenceTxt.Connect("pressed", this, "onCompetencePressed");

        Connect("visibility_changed", this, "onVisibilityChanged");

        Control progressContainer = GetNode<Control>("VBoxContainer");
        progressTxt = progressContainer.GetNode<RichTextLabel>("RichTextLabel");
        progressBar = progressContainer.GetNode<ProgressBar>("TextureProgress");
    }
    public void onVisibilityChanged()
    {
        if(!Visible)return;
        SaveData savedata = gh.saveData;
        Stats stats = (Stats)savedata.stats;
        Talisman talisman = (Talisman)stats.talisman;
        //Update main stats
        healthTxt.Text = stats.baseMaxHealth+ AddExtra(talisman.health);
        regenHealthTxt.Text = stats.baseHealthRegen + AddExtra(talisman.healthRegen);
        inspirationTxt.Text = stats.baseMaxInspiration + AddExtra(talisman.inspiration);
        regenInspirationTxt.Text = stats.baseInspirationRegen + AddExtra(talisman.inspirationRegen);
        strengthTxt.Text = stats.baseStrength + AddExtra(talisman.strength);
        defenseTxt.Text = stats.baseDefense + AddExtra(talisman.defense);
        competenceTxt.Text = stats.baseCompetence + AddExtra(talisman.competence);

        //Update level
        int level = stats.level;
        int xp = stats.xp;
        int maxXp = stats.levelThreshold[level];

        progressTxt.Text = "Lv." + level + " " + xp + "/" + maxXp;
        progressBar.Value = xp;
        progressBar.MaxValue = maxXp;
    }

    public string AddExtra(int data)
    {
        if (data == 0) return "";
        string prefix = data < 0 ? "" : "+";
        string str = "(" + prefix + data + ")";
        return str;
    }
    public void onHealthPressed()
    {
        desc.BbcodeText = "[color=#1ebc73]Health: [color=#fff]Determines how much health you have in battle";
    }
    public void onRegenHealthPressed()
    {
        desc.BbcodeText = "[color=#91db69]Health Regen: [color=#fff]The ammount of health restored when regenerating";
    }
    public void onInspirationPressed()
    {
        desc.BbcodeText = "[color=#f79617]Inspiration: [color=#fff]A requirement for doing actions in battle";
    }
    public void onRegenInspirationPressed()
    {
        desc.BbcodeText = "[color=#f9c22b]Inspiration Regen: [color=#fff]The ammount of Inspiration restored when regenerating";
    }
    public void onStrengthPressed()
    {
        desc.BbcodeText = "[color=#ea4f36]Strength: [color=#fff]Provides more damage to your attacks";
    }
    public void onDefensePressed()
    {
        desc.BbcodeText = "[color=#4d9be6]Defense: [color=#fff]Reduces damage when hit";
    }
    public void onCompetencePressed()
    {
        desc.BbcodeText = "[color=#905ea9]Competence: [color=#fff]Determines the hit window for each beat. It will be easier to hit on beats the higher the competence";
    }


}
