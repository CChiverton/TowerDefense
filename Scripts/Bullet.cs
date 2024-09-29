using Godot;
using System;

public partial class Bullet : Area2D
{
	public Vector2 Direction = Vector2.Zero;
	private int Speed = 300;
	[Export]
	private int _damage {get; set;}
	
	[Signal]		// Called when the bullet collides with an enemy
	public delegate void EnemyHitEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EnemyHit += GetNode<Tower>("../..").AddTowerMovement;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += (float)delta * Speed * Direction;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	private void OnAreaEntered(Node2D body)
	{
		if (body.IsInGroup("Enemies"))
		{
			if (body.HasMethod("BulletHit"))
			{
				if ((bool)body.Call("BulletHit", _damage))
				{
					EmitSignal(SignalName.EnemyHit);
				}
			}
			QueueFree();
		}
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
