using Godot;
using System;

public partial class Bullet : Area2D
{
	public Vector2 Direction = Vector2.Zero;
	private int Speed = 300;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += (float)delta * Speed * Direction;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
