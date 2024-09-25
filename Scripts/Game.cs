using Godot;
using System;

public partial class Game : Node2D
{
	public int Lives = 100;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Lives = 100;
		GetNode<LifeCounter>("UI/LifeCounter").SetLives(Lives);
		GetNode<Timer>("EnemyTimer").Start();
	}

	/************* Game functions *************/
	
	private void OnPlayerZoneLifeLoss()
	{
		Lives -= 1;
		GetNode<LifeCounter>("UI/LifeCounter").SetLives(Lives);
	}
	
	public void SpawnBullet(PackedScene bullet, Vector2 direction, Vector2 position)
	{
		var SpawnedBullet = bullet.Instantiate<Bullet>();
		SpawnedBullet.Direction = direction;
		SpawnedBullet.GlobalPosition = position;
		AddChild(SpawnedBullet);
	}
	
	[Export]
	public PackedScene EnemyScene {get; set;}
	public void OnEnemyTimerTimeout()
	{
		var enemy = EnemyScene.Instantiate<Path2D>();
		AddChild(enemy);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
