using Godot;
using System;

public partial class LevelOne : Node2D
{
	[Export]
	private PackedScene _basicEnemy {get; set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("SpawnDelayTimer").Start();
	}
	private void SpawnBasicEnemy()
	{
		var enemy = _basicEnemy.Instantiate<PathFollow2D>();
		GetNode("Path2D").AddChild(enemy);
	}
	
	public void OnSpawnDelayTimerTimeout()
	{
		GetNode<Timer>("WaveOneTimer").Start();
	}
	
	private void OnWaveOneTimerTimeout()
	{
		SpawnBasicEnemy();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
