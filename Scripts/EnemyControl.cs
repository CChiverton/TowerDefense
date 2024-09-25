using Godot;
using System;

// Controls the movement of units and their destruction
public partial class EnemyControl : Path2D
{
	private PathFollow2D _path;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_path = GetNode<PathFollow2D>("EnemyPath");
	}
	
	private void OnEnemyUnitHit()
	{
		QueueFree();
	}
	
		// Path following code to move unit along
	private void Move(double delta)
	{
		_path.SetProgress(_path.GetProgress() + GetNode<Enemy>("EnemyPath/Enemy").Speed * (float)delta);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Move(delta);
	}
}
