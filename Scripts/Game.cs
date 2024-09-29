using Godot;
using System;

public partial class Game : Node2D
{
	// Game stats
	public int Lives = 100;
	public int Gold = 100;
	private int _score = 0;
	public int BoardValue = 0;
	
	//UI
	private LifeCounter _lifeCounter;
	private GoldCounter _goldCounter;
	private ScoreCounter _scoreCounter;
	private BoardValueCounter _boardValueCounter;
	private PackedScene _gameOver = GD.Load<PackedScene>("res://Scenes/GameOverScreen.tscn");
	private bool _lossScreen = false;
	private PackedScene _pausedScreen = GD.Load<PackedScene>("res://Scenes/PauseScreen.tscn");
	private bool _paused = false;
	
	// Tower building
	private bool _buildMode;
	private bool _debounce;
	private Tower TowerBuild;
	private int _cost;
	
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Lives = 100;
		Gold = 100;
		_score = 0;
		BoardValue = 0;
		_lifeCounter = GetNode<LifeCounter>("UICanvas/UI/LifeCounter");
		_goldCounter = GetNode<GoldCounter>("UICanvas/UI/GoldCounter");
		_scoreCounter = GetNode<ScoreCounter>("UICanvas/UI/ScoreCounter");
		_boardValueCounter = GetNode<BoardValueCounter>("UICanvas/UI/BoardValueCounter");
		_lifeCounter.SetLives(Lives);
		_goldCounter.SetGold(Gold);
		_scoreCounter.SetScore(_score);
		_boardValueCounter.SetValue(BoardValue);
		_buildMode = false;
		_debounce = false;
		_cost = 0;
	}

	/************* Game functions *************/
	
	public void OnPlayerZoneLifeLoss()
	{
		if (_lossScreen == false)
		{
			Lives -= 1;
			_lifeCounter.SetLives(Lives);
			if ((Lives <= 0))
			{
				GameOverScreen GameOver = _gameOver.Instantiate<GameOverScreen>();
				AddChild(GameOver);
				_lossScreen = true;
			}
		}
	}
	
	public void ChangeGold(int gold)
	{
		Gold += gold;
		_goldCounter.SetGold(Gold);
	}
	
	public void IncrementScore()
	{
		_score++;
		_scoreCounter.SetScore(_score);
	}
	
	private void SetValue(int value)
	{
		BoardValue += value;
		_boardValueCounter.SetValue(BoardValue);
	}
	
	private void BuildModeReset()
	{
		TowerBuild = null;
		_buildMode = false;
		_debounce = false;
	}
	
	public void BuildingStart(PackedScene scene, int cost)
	{
		_buildMode = true;
		_debounce = false;
		Tower Tower = scene.Instantiate<Tower>();
		Tower.Position = GetGlobalMousePosition();
		Tower.SetAttackRangeVisibility(true);
		GetNode("Towers").AddChild(Tower);
		Tower.TargetingActive = false;
		TowerBuild = Tower;
		_cost = cost;
	}
	
	private void BuildingStop()
	{
		if (_buildMode)
		{
			if (Input.IsActionJustPressed("MouseRight") || Input.IsActionJustPressed("Escape"))
			{
				BuildModeReset();
				GetNode("Towers").GetChild(-1).QueueFree();
			}
		}
	}
	
	private void BuildTower()
	{
		if (_buildMode)
		{
			TowerBuild.Position = GetGlobalMousePosition();
			
			if (Input.IsActionJustPressed("MouseLeft") && _debounce)
			{
				TowerBuild.SetAttackRangeVisibility(false);
				TowerBuild.TargetingActive = true;
				ChangeGold(-_cost);
				SetValue(_cost);
				BuildModeReset();
			}
			_debounce = true;
		}
	}

	private void GetInput()
	{
		if (!_buildMode)
		{
			if (Input.IsActionJustPressed("Escape"))
			{
				if (_paused)
				{
					GetNode<GameOverScreen>("PauseScreen").QueueFree();
					_paused = false;
				} else {
					GameOverScreen Pause = _pausedScreen.Instantiate<GameOverScreen>();
					AddChild(Pause);
					_paused = true;
				}
			}
		}
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetInput();
		BuildingStop();
		BuildTower();
	}
}
