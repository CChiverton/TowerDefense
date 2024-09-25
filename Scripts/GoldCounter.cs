using Godot;
using System;

public partial class GoldCounter : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void SetGold(int gold)
	{
		Text = $"Gold: {gold}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
