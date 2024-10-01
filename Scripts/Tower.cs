using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Area2D
{
	private float _posX, _posY;
	private int _health = 50;
	[Export]
	private float _attackRange {get; set;}
	private Timer ShootCooldown;
	private float _movement = 0;
	private float _backMove = 5;
	private float _forwardMove = 15;
	private float _enemyCollision = 50;
	private float _speed = 20;
	public bool TargetingActive = true;
	private List<Node2D> _enemiesInRange = new List<Node2D>();
	[Signal]
	public delegate void EnemyKilledEventHandler();
	
	private PackedScene _bullet = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_posX = GlobalPosition.X;
		_posY = GlobalPosition.Y;
		
		ShootCooldown = GetNode<Timer>("ShootTimer");
		EnemyKilled += GetNode<Game>("/root/Game").IncrementScore;
		CircleShape2D AttackRange = (CircleShape2D)GetNode<CollisionShape2D>("AttackRange/AttackRangeCollider").Shape;
		AttackRange.SetRadius(_attackRange);
		Sprite2D AttackRangeIndicator = (Sprite2D)GetNode<Sprite2D>("AttackRange/Sprite2D");
		AttackRangeIndicator.Scale = (new Vector2((_attackRange*2)/AttackRangeIndicator.Texture.GetWidth(),(_attackRange*2)/AttackRangeIndicator.Texture.GetHeight()));
		
	}
	
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
		if (ShootCooldown.IsStopped())
		{
			SpawnBullet(GlobalPosition.DirectionTo(enemy.GlobalPosition));
			ShootCooldown.Start(ShootCooldown.WaitTime);
		}
	}
	
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
	
	private void OnAttackRangeAreaEntered(Node2D body)
	{
		if (body.IsInGroup("Enemies"))
		{
			_enemiesInRange.Add(body);
		}
	}
	
	private void OnAttackRangeAreaExited(Node2D body)
	{
		if (body.IsInGroup("Enemies"))
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
			Node2D NearestEnemy = null;
			float NearestDistance = 1000;

			foreach (Node2D enemy in GetTree().GetNodesInGroup("Enemies"))
			{
				float Distance = Position.DistanceTo(enemy.GlobalPosition);

				if (Distance < NearestDistance)
				{
					NearestDistance = Distance;
					NearestEnemy = enemy;
				}
			}
			if (NearestDistance < _attackRange)
			{
				Shoot(NearestEnemy);
			}
		}
	}
	
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
