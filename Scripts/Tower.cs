using Godot;
using System;

public partial class Tower : Area2D
{
	private float _posX, _posY;
	private float _attackRange = 150;
	private Timer ShootCooldown;

	private PackedScene _bullet = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	[Signal]
	public delegate void FireBulletEventHandler(PackedScene bullet, Vector2 direction, Vector2 position);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_posX = GlobalPosition.X;
		_posY = GlobalPosition.Y;

		ShootCooldown = GetNode<Timer>("ShootTimer");
		FireBullet += GetNode<Game>("/root/Game").SpawnBullet;
	}
	
	private void Shoot(Node2D enemy)
	{
		if (ShootCooldown.IsStopped())
		{
			EmitSignal(SignalName.FireBullet, _bullet, GlobalPosition.DirectionTo(enemy.GlobalPosition), GlobalPosition);
			ShootCooldown.Start(ShootCooldown.WaitTime);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Node2D NearestEnemy = null;
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
			Shoot(NearestEnemy);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
