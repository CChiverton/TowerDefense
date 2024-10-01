using Godot;
using System;

public partial class Enemy : Area2D
{
	public float Speed = 50.0F;
	[Export]
	private float _health {get; set;}
	private ProgressBar HealthBar;

	[Signal]
	public delegate void PlayerBaseReachedEventHandler();
	[Signal]		// Connects to EnemyControl.cs to handle unit destruction
	public delegate void UnitHitEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_health = _health * (1 +(float)(GetNode<Game>("/root/Game").BoardValue / 500.0F));
		HealthBar = GetNode<ProgressBar>("HealthBar");
		HealthBar.MaxValue = _health;
		HealthBar.Visible = false;
	}

	private void OnAreaEntered(Node2D body)
	{
		if (body.IsInGroup("Players"))
		{
			if (body.HasMethod("RemoveLife"))
			{
				body.Call("RemoveLife");
				EmitSignal(SignalName.PlayerBaseReached);
			}
		}
	}
	
	// Called by a bullet colliding with this unit
	public bool BulletHit(int damage)
	{
		HealthBar.Visible = true;
		if ((_health -= damage) <= 0)
		{
			EmitSignal(SignalName.UnitHit);
			return true;
		}
		return false;
	}
	
	private void UpdateHealthBar()
	{
		HealthBar.Value = _health;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateHealthBar();
	}
}
