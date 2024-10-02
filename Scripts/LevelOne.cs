using Godot;
using System;

public partial class LevelOne : Node2D
{
	[Export]
	private PackedScene _basicEnemy {get; set;}
	[Export]
	private PackedScene _slowEnemy {get; set;}
	[Export]
	private PackedScene _fastEnemy {get; set;}
	
	private Timer _spawnDelay;
	private Timer _basicSpread;
	private Timer _basicNormal;
	private Timer _basicPacked;
	private Timer _slowSpread;
	private Timer _slowNormal;
	private Timer _slowPacked;
	private Timer _fastSpread;
	private Timer _fastNormal;
	private Timer _fastPacked;
	
	private int _wave = 0;
	private int _basicSpawns = 0;
	private int _basicSpawnNumber = 0;
	private int _slowSpawns = 0;
	private int _slowSpawnNumber = 0;
	private int _fastSpawns = 0;
	private int _fastSpawnNumber = 0;
	
	private bool _waveCleared = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnDelay = GetNode<Timer>("SpawnDelayTimer");
		_spawnDelay.Start();
		
		_basicSpread = GetNode<Timer>("Waves/BasicSpreadTimer");
		_basicNormal = GetNode<Timer>("Waves/BasicNormalTimer");
		_basicPacked = GetNode<Timer>("Waves/BasicPackedTimer");
		_slowSpread = GetNode<Timer>("Waves/SlowSpreadTimer");
		_slowNormal = GetNode<Timer>("Waves/SlowNormalTimer");
		_slowPacked = GetNode<Timer>("Waves/SlowPackedTimer");
		_fastSpread = GetNode<Timer>("Waves/FastSpreadTimer");
		_fastNormal = GetNode<Timer>("Waves/FastNormalTimer");
		_fastPacked = GetNode<Timer>("Waves/FastPackedTimer");
	}
	
	private void SetBasicSpawns(int num)
	{
		_basicSpawns = 0;
		_basicSpawnNumber = num;
	}
	
	private void SetSlowSpawns(int num)
	{
		_slowSpawns = 0;
		_slowSpawnNumber = num;
	}
	
	private void SetFastSpawns(int num)
	{
		_fastSpawns = 0;
		_fastSpawnNumber = num;
	}
	
	private void SpawnBasicEnemy()
	{
		var enemy = _basicEnemy.Instantiate<PathFollow2D>();
		GetNode("Path2D").AddChild(enemy);
		_basicSpawns++;
	}
	
	private void SpawnSlowEnemy()
	{
		var enemy = _slowEnemy.Instantiate<PathFollow2D>();
		GetNode("Path2D").AddChild(enemy);
		_slowSpawns++;
	}
	
	private void SpawnFastEnemy()
	{
		var enemy = _fastEnemy.Instantiate<PathFollow2D>();
		GetNode("Path2D").AddChild(enemy);
		_fastSpawns++;
	}
	
	public void OnSpawnDelayTimerTimeout()
	{
		if (_waveCleared)
		{
			switch(++_wave)
			{
				case 1:
					SetBasicSpawns(10);
					_basicNormal.Start();
					break;
				case 2:
					SetBasicSpawns(15);
					_basicSpread.Start();
					break;
				case 3:
					SetBasicSpawns(5);
					_basicPacked.Start();
					break;
				case 4:
					SetSlowSpawns(5);
					_slowNormal.Start();
					break;
				case 5:
					SetFastSpawns(10);
					_fastSpread.Start();
					break;
				case 6:
					SetSlowSpawns(5);
					_slowPacked.Start();
					SetFastSpawns(10);
					_fastPacked.Start();
					break;
				case 7:
					SetSlowSpawns(10);
					_slowNormal.Start();
					SetBasicSpawns(10);
					_basicNormal.Start();
					break;
				case 8:
					SetSlowSpawns(5);
					_slowSpread.Start();
					SetBasicSpawns(10);
					_basicSpread.Start();
					SetFastSpawns(5);
					_fastPacked.Start();
					break;
				case 9:
					SetBasicSpawns(10);
					_basicPacked.Start();
					SetFastSpawns(20);
					_fastSpread.Start();
					break;
				case 10:
					SetSlowSpawns(15);
					_slowSpread.Start();
					SetBasicSpawns(20);
					_basicNormal.Start();
					break;
				default:
					GD.Print("Wave number unrecognised");
					break;
			}
			_waveCleared = false;
		} else {
			if (GetNode("Path2D").GetChildCount() == 0) _waveCleared = true;
			_spawnDelay.Start();
		}
	}
	
	private bool BasicSpawnCheck()
	{
		if (_basicSpawns == _basicSpawnNumber)
		{
			_spawnDelay.Start();
			_basicSpawns = 0;
			return true;
		}
		return false;
	}
	
	private bool SlowSpawnCheck()
	{
		if (_slowSpawns == _slowSpawnNumber)
		{
			_spawnDelay.Start();
			_slowSpawns = 0;
			return true;
		}
		return false;
	}
	
	private bool FastSpawnCheck()
	{
		if (_fastSpawns == _fastSpawnNumber)
		{
			_spawnDelay.Start();
			_fastSpawns = 0;
			return true;
		}
		return false;
	}
	
	private void OnBasicSpreadTimerTimeout()
	{
		SpawnBasicEnemy();
		if (BasicSpawnCheck())
		{
			_basicSpread.Stop();
		}
	}
	
	private void OnBasicNormalTimerTimeout()
	{
		SpawnBasicEnemy();
		if (BasicSpawnCheck())
		{
			_basicNormal.Stop();
		}
	}
	
	private void OnBasicPackedTimerTimeout()
	{
		SpawnBasicEnemy();
		if (BasicSpawnCheck())
		{
			_basicPacked.Stop();
		}
	}

	private void OnSlowSpreadTimerTimeout()
	{
		SpawnSlowEnemy();
		if (SlowSpawnCheck())
		{
			_slowSpread.Stop();
		}
	}
	
	private void OnSlowNormalTimerTimeout()
	{
		SpawnSlowEnemy();
		if (SlowSpawnCheck())
		{
			_slowNormal.Stop();
		}
	}
	
	private void OnSlowPackedTimerTimeout()
	{
		SpawnSlowEnemy();
		if (SlowSpawnCheck())
		{
			_slowPacked.Stop();
		}
	}
	
	private void OnFastSpreadTimerTimeout()
	{
		SpawnFastEnemy();
		if (FastSpawnCheck())
		{
			_fastSpread.Stop();
		}
	}
	
	private void OnFastNormalTimerTimeout()
	{
		SpawnFastEnemy();
		if (FastSpawnCheck())
		{
			_fastNormal.Stop();
		}
	}
	
	private void OnFastPackedTimerTimeout()
	{
		SpawnFastEnemy();
		if (FastSpawnCheck())
		{
			_fastPacked.Stop();
		}
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
