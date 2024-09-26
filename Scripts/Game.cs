using Godot;
using System;

public partial class Game : Node2D
{
	public int Lives = 100;
	public int Gold = 100;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Lives = 100;
		Gold = 100;
		GetNode<LifeCounter>("UI/LifeCounter").SetLives(Lives);
		GetNode<GoldCounter>("UI/GoldCounter").SetGold(Gold);
	}

	/************* Game functions *************/
	
	public void OnPlayerZoneLifeLoss()
	{
		Lives -= 1;
		GetNode<LifeCounter>("UI/LifeCounter").SetLives(Lives);
	}
	
	public void ChangeGold(int gold)
	{
		Gold += gold;
		GetNode<GoldCounter>("UI/GoldCounter").SetGold(Gold);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
