using Godot;
using System;

public partial class Tower : Area2D
{
	private float _posX, _posY;
	private float _attackRange = 150;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_posX = GlobalPosition.X;
		_posY = GlobalPosition.Y;
	}

	public override void _PhysicsProcess(double delta)
	{
		Node2D NearestEnemy;
		float NearestDistance = 1000;

		foreach (Node2D enemy in GetTree().GetNodesInGroup("Enemies"))
		{
			float Distance = Position.DistanceTo(enemy.GlobalPosition);

			if (Distance < NearestDistance)
			{
				NearestDistance = Distance;
				NearestEnemy = enemy;
			}
		}
		if (NearestDistance < _attackRange)
		{
			GD.Print("Enemy Within Range");
			// Fire at enemy
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
