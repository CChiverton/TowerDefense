using Godot;
using System;

public partial class Game : Node2D
{
	public int Lives = 100;
	public int Gold = 100;
	private int _score = 0;
	public int BoardValue = 0;
	private LifeCounter _lifeCounter;
	private GoldCounter _goldCounter;
	private ScoreCounter _scoreCounter;
	private BoardValueCounter _boardValueCounter;
	
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
		Lives -= 1;
		_lifeCounter.SetLives(Lives);
		if (Lives <= 0)
		{
			GetTree().ReloadCurrentScene();
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
		GetNode("Towers").AddChild(Tower);
		Tower.TargetingActive = false;
		TowerBuild = Tower;
		_cost = cost;
	}
	
	private void BuildingStop()
	{
		if (Input.IsActionJustPressed("MouseRight") || Input.IsActionJustPressed("Escape"))
		{
			BuildModeReset();
			GetNode("Towers").GetChild(-1).QueueFree();
		}
	}
	
	private void BuildTower()
	{
		if (_buildMode)
		{
			TowerBuild.Position = GetGlobalMousePosition();
			
			if (Input.IsActionJustPressed("MouseLeft") && _debounce)
			{
				TowerBuild.TargetingActive = true;
				ChangeGold(-_cost);
				SetValue(_cost);
				BuildModeReset();
			}
			_debounce = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		BuildingStop();
		BuildTower();
	}
}
