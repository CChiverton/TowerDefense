using Godot;
using System;

public partial class Game : Node2D
{
	public int Lives = 100;
	public int Gold = 100;
	private LifeCounter _lifeCounter;
	private GoldCounter _goldCounter;
	private bool _buildMode;
	private bool _debounce;
	private Tower TowerBuild;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Lives = 100;
		Gold = 100;
		_lifeCounter = GetNode<LifeCounter>("UICanvas/UI/LifeCounter");
		_goldCounter = GetNode<GoldCounter>("UICanvas/UI/GoldCounter");
		_lifeCounter.SetLives(Lives);
		_goldCounter.SetGold(Gold);
		_buildMode = false;
		_debounce = false;
	}

	/************* Game functions *************/
	
	public void OnPlayerZoneLifeLoss()
	{
		Lives -= 1;
		_lifeCounter.SetLives(Lives);
	}
	
	public void ChangeGold(int gold)
	{
		Gold += gold;
		_goldCounter.SetGold(Gold);
	}
	
	public void BuildingStart(PackedScene scene)
	{
		Tower Tower = scene.Instantiate<Tower>();
		Tower.Position = GetGlobalMousePosition();
		GetNode("Towers").AddChild(Tower);
		Tower.TargetingActive = false;
		TowerBuild = Tower;
		_buildMode = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_buildMode)
		{
			TowerBuild.Position = GetGlobalMousePosition();
			
			if (Input.IsActionJustPressed("MouseLeft") && _debounce)
			{
				GD.Print("Mouse Pressed");
				TowerBuild.TargetingActive = true;
				ChangeGold(-TowerBuild.Cost);
				TowerBuild = null;
				_buildMode = false;
				_debounce = false;
			}
			_debounce = true;
		}
	}
}
