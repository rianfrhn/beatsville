using Godot;
using System;

public class HealthBar : Control
{
	TextureProgress healthbar;
	TextureProgress inspirationbar;
	public TextureRect picture;

	public override void _Ready()
	{
		healthbar = GetNode<TextureProgress>("HealthBar");
		inspirationbar = GetNode<TextureProgress>("InspirationBar");
		picture = GetNode<TextureRect>("Frame/TextureRect");
	}

	public void onHealthChanged(int health, int maxHealth)
	{
		healthbar.MaxValue = maxHealth;
		healthbar.Value = health;
		GD.Print("healthbar updated");
	}
	public void onHealthUpdated(int maxHealth)
	{
		healthbar.MaxValue = maxHealth;
	}
	public void onInspirationChanged(int inspiration, int maxInspiration)
	{
		inspirationbar.MaxValue = maxInspiration;
		inspirationbar.Value = inspiration;

	}
	public void onInspirationUpdated(int maxInspiration)
	{

		inspirationbar.MaxValue = maxInspiration;
	}

}

