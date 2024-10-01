using Godot;
using System;

public partial class LevelOne : Node2D
{
	[Export]
	private PackedScene _basicEnemy {get; set;}
	
	private Timer _spawnDelay;
	private Timer _basicSpread;
	private Timer _basicNormal;
	private Timer _basicPacked;
	
	private int _wave = 0;
	private int _spawns = 0;
	private int _spawnNumber = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnDelay = GetNode<Timer>("SpawnDelayTimer");
		_spawnDelay.Start();
		
		_basicSpread = GetNode<Timer>("Waves/BasicSpreadTimer");
		_basicNormal = GetNode<Timer>("Waves/BasicNormalTimer");
		_basicPacked = GetNode<Timer>("Waves/BasicPackedTimer");
	}
	
	private void SetSpawns(int num)
	{
		_spawns = 0;
		_spawnNumber = num;
	}
	
	private void SpawnBasicEnemy()
	{
		var enemy = _basicEnemy.Instantiate<PathFollow2D>();
		GetNode("Path2D").AddChild(enemy);
		_spawns++;
	}
	
	public void OnSpawnDelayTimerTimeout()
	{
		_spawns = 0;
		switch(++_wave)
		{
			case 1:
				SetSpawns(10);
				_basicNormal.Start();
				break;
			case 2:
				SetSpawns(20);
				_basicSpread.Start();
				break;
			case 3:
				SetSpawns(5);
				_basicPacked.Start();
				break;
			default:
				GD.Print("Wave number unrecognised");
				break;
		}
	}
	
	private void OnBasicSpreadTimerTimeout()
	{
		SpawnBasicEnemy();
		if (_spawns == _spawnNumber)
		{
			_basicSpread.Stop();
			_spawnDelay.Start();
		}
	}
	
	private void OnBasicNormalTimerTimeout()
	{
		SpawnBasicEnemy();
		if (_spawns == _spawnNumber)
		{
			_basicNormal.Stop();
			_spawnDelay.Start();
		}
	}
	
	private void OnBasicPackedTimerTimeout()
	{
		SpawnBasicEnemy();
		if (_spawns == _spawnNumber)
		{
			_basicPacked.Stop();
			_spawnDelay.Start();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
