using Godot;
using System;

public partial class LifeCounter : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void SetLives(int lives)
	{
		Text = $"Lives: {lives}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
