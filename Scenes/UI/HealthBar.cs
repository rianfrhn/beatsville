using Godot;
using System;

public class HealthBar : Control
{
	ProgressBar healthbar;
	ProgressBar inspirationbar;

	public override void _Ready()
	{
		healthbar = GetNode<ProgressBar>("HealthBar");
		inspirationbar = GetNode<ProgressBar>("InspirationBar");
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









