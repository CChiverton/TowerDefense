using Godot;
using System;

public partial class Enemy : Area2D
{
	public float Speed = 50.0F;

	[Signal]
	public delegate void PlayerBaseReachedEventHandler();
	[Signal]
	public delegate void UnitHitEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void OnAreaEntered(Node2D body)
	{
		if (body.HasMethod("RemoveLife"))
		{
			body.Call("RemoveLife");
			EmitSignal(SignalName.UnitHit);
		}
	}
	
	public void BulletHit()
	{
		EmitSignal(SignalName.UnitHit);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
