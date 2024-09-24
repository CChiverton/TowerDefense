using Godot;
using System;

public partial class Game : Node2D
{
	public int Lives = 100;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Lives = 100;
		GetNode<LifeCounter>("UI/LifeCounter").SetLives(Lives);
	}

	private void OnPlayerZoneLifeLoss()
	{
		Lives -= 1;
		GetNode<LifeCounter>("UI/LifeCounter").SetLives(Lives);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
