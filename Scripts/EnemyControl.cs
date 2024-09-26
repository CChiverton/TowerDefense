using Godot;
using System;

// Controls the movement of units and their destruction
public partial class EnemyControl : PathFollow2D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void OnEnemyPlayerBaseReached()
	{
		QueueFree();
	}
	
	private void OnEnemyUnitHit()
	{
		GetNode<Game>("/root/Game").ChangeGold(1);
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		SetProgress(GetProgress() + GetNode<Enemy>("Enemy").Speed * (float)delta);
	}
}
