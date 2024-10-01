using Godot;
using System;

public partial class HealthMultiplierCounter : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void SetMultiplier(float multiplier)
	{
		Text = $"Health Multiplier: {multiplier}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
