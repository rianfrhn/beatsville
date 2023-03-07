using Godot;
using System;

public class ProfileUI : VBoxContainer
{
    Control statsBox;
    GlobalVariable gv;
    Button
        healthTxt, regenHealthTxt,
        inspirationTxt, regenInspirationTxt,
        strengthTxt, defenseTxt, competenceTxt;

    RichTextLabel progressTxt;
    ProgressBar progressBar;

    RichTextLabel desc;

    public override void _Ready()
    {
        gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
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
        SaveData savedata = gv.saveData;
        Stats stats = (Stats)savedata.stats;
        //Update main stats
        healthTxt.Text = stats.maxHealth.ToString();
        regenHealthTxt.Text = stats.healthRegen.ToString();
        inspirationTxt.Text = stats.maxInspiration.ToString();
        regenInspirationTxt.Text = stats.inspirationRegen.ToString();
        strengthTxt.Text = stats.strength.ToString();
        defenseTxt.Text = stats.defense.ToString();
        competenceTxt.Text = stats.competence.ToString();

        //Update level
        int level = stats.level;
        int xp = stats.xp;
        int maxXp = stats.levelThreshold[level];

        progressTxt.Text = "Lv." + level + " " + xp + "/" + maxXp;
        progressBar.Value = xp;
        progressBar.MaxValue = maxXp;
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
