using Godot;
using System;

public partial class EndZone : Area2D
{
	[Signal]
	public delegate void LifeLossEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	// Called whenever an enemy enters the PlayerZone
	public void RemoveLife()
	{
		EmitSignal(SignalName.LifeLoss);
	}
	
	// Called when a tower enters the EnemyZone
	public void DestroyTower()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
