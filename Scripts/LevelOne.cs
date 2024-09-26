using Godot;
using System;

public partial class LevelOne : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("EnemyTimer").Start();
	}
	
	[Export]
	public PackedScene EnemyScene {get; set;}
	public void OnEnemyTimerTimeout()
	{
		var enemy = EnemyScene.Instantiate<PathFollow2D>();
		GetNode("Path2D").AddChild(enemy);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
