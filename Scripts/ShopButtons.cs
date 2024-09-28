using Godot;
using System;

public partial class ShopButtons : Button
{
	[Export]
	public PackedScene TowerScene {get; set;}
	[Signal]
	public delegate void BuildModeEventHandler(PackedScene scene, int cost);
	
	private int _cost = 50;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BuildMode += GetNode<Game>("/root/Game").BuildingStart;
	}
	
	private void OnButtonDown()
	{
		if (GetNode<Game>("/root/Game").Gold >= _cost)
		{
			EmitSignal(SignalName.BuildMode, TowerScene, _cost);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
