using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Area2D
{
	/***** Tower stats *****/
	[Export]
	private int _health {get; set;}
	private ProgressBar _healthBar;
	// Tower Attacking
	[Export]
	private float _attackRange {get; set;}
	private Timer _shootCooldown;
	public bool TargetingActive = true;
	private List<Node2D> _enemiesInRange = new List<Node2D>();
	private PackedScene _bullet = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	//Tower Movement
	private float _movement = 0;
	private float _backMove = 5;
	private float _forwardMove = 15;
	private float _enemyCollision = 50;
	private float _speed = 20;
	
	[Signal]
	public delegate void EnemyKilledEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shootCooldown = GetNode<Timer>("ShootTimer");
		EnemyKilled += GetNode<Game>("/root/Game").IncrementScore;
		
		CircleShape2D AttackRange = (CircleShape2D)GetNode<CollisionShape2D>("AttackRange/AttackRangeCollider").Shape;
		AttackRange.SetRadius(_attackRange);
		Sprite2D AttackRangeIndicator = (Sprite2D)GetNode<Sprite2D>("AttackRange/Sprite2D");
		AttackRangeIndicator.Scale = (new Vector2((_attackRange*2)/AttackRangeIndicator.Texture.GetWidth(),(_attackRange*2)/AttackRangeIndicator.Texture.GetHeight()));
		
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthBar.MaxValue = _health;
		_healthBar.Visible = false;
	}
	
	/************************ Tower Attacking ***********************/
	
	public void SetAttackRangeVisibility(bool active)
	{
		Sprite2D AttackRangeIndicator = (Sprite2D)GetNode<Sprite2D>("AttackRange/Sprite2D");
		AttackRangeIndicator.Visible = active;
	}
	
	private void SpawnBullet(Vector2 direction)
	{
		var SpawnedBullet = _bullet.Instantiate<Bullet>();
		SpawnedBullet.Direction = direction;
		SpawnedBullet.Position = GlobalPosition;				// Spawn on the parent
		GetNode<Node>("Bullets").AddChild(SpawnedBullet);       // Spawn in group to decouple position from parent
	}
	
	private void Shoot(Node2D enemy)
	{
		if (_shootCooldown.IsStopped())
		{
			SpawnBullet(GlobalPosition.DirectionTo(enemy.GlobalPosition));
			_shootCooldown.Start(_shootCooldown.WaitTime);
		}
	}
	
	private void OnAttackRangeAreaEntered(Node2D body)
	{
		if (body.IsInGroup("Enemies") && body.IsInGroup("Units"))
		{
			_enemiesInRange.Add(body);
		}
	}
	
	private void OnAttackRangeAreaExited(Node2D body)
	{
		if (body.IsInGroup("Enemies") && body.IsInGroup("Units"))
		{
			_enemiesInRange.Remove(body);
			float Distance = Position.DistanceTo(body.GlobalPosition);
			
			if (Distance < _attackRange)	// Enemy died within range
			{
				_movement -= _backMove;
			}
		}
	}
	
	private void TargetEnemy()
	{
		if (TargetingActive)
		{
			if (_enemiesInRange.Count > 0)
			{
				Node2D NearestEnemy = null;
				float NearestDistance = _attackRange * 2;  // Distance to the centre of an enemy is greater than to the edge

				foreach (Node2D enemy in _enemiesInRange)
				{
					float Distance = Position.DistanceTo(enemy.GlobalPosition);
					
					if (Distance < NearestDistance)
					{
						NearestDistance = Distance;
						NearestEnemy = enemy;
					}
				}

				Shoot(NearestEnemy);
			}
		}
	}
	
	/********************************* Tower Movement *******************************/
	public void AddTowerMovement()
	{
		_movement += _forwardMove;
		EmitSignal(SignalName.EnemyKilled);
	}
	
	private void MoveTower(double delta)
	{
		float MaxMovement = (float)delta * _speed;
		if (_movement > 0)
		{
			if (MaxMovement > _movement)
			{
				Position -= new Vector2(_movement, 0);
				_movement = 0;
			} else {
				Position -= new Vector2(MaxMovement, 0);
				_movement -= MaxMovement;
			}
		} else if (_movement < 0) {
			MaxMovement = -MaxMovement;
			if (MaxMovement > _movement)
			{
				Position -= new Vector2(MaxMovement, 0);
				_movement -= MaxMovement;
			} else {
				Position -= new Vector2(_movement, 0);
				_movement = 0;
			}
		}
	}
	
	
	/************************** Tower Signal *************************/
	private void OnAreaEntered(Node2D body)
	{
		if (TargetingActive)	// Ensures that the 
		{
			if (body.IsInGroup("Enemies"))
			{
				// On colliding with an Endzone
				if (body.HasMethod("DestroyTower"))
				{
					body.Call("DestroyTower");
					QueueFree();
				}
				// On colliding with an enemy unit
				if (body.HasMethod("BulletHit"))
				{
					if ((bool)body.Call("BulletHit", 10000000))	// Guarantee enemy death
					{
						if ((_health -= 10) <= 0)
						{
							QueueFree();
						}
						_healthBar.Visible = true;
						_healthBar.Value = _health;
						_movement -= _enemyCollision;
					}
				}
			}
			
			if (body.IsInGroup("Players"))
			{
				if (body.HasMethod("DestroyTower"))
				{
					body.Call("DestroyTower");
					QueueFree();
				}
			}
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		TargetEnemy();
		MoveTower(delta);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
