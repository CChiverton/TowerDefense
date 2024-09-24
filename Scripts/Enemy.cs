using Godot;
using System;

public partial class Enemy : Area2D
{
	private PathFollow2D _path;
	private float _speed = 50.0F;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_path = GetParent<PathFollow2D>();
	}

	private void Follow(double delta)
	{
		_path.SetProgress(_path.GetProgress() + _speed * (float)delta);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Follow(delta);
	}
}
