using Godot;
using System;

public partial class PlayerZone : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	// Called whenever an enemy enters this zone
	public void RemoveLife()
	{
		GD.Print("Removing Life");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
