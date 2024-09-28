using Godot;
using System;

public partial class ShopButtons : Button
{
	[Export]
	public PackedScene TowerScene {get; set;}
	[Signal]
	public delegate void BuildModeEventHandler(PackedScene scene);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BuildMode += GetNode<Game>("/root/Game").BuildingStart;
	}
	
	private void OnButtonDown()
	{
		EmitSignal(SignalName.BuildMode, TowerScene);
		GD.Print("Button has been pressed");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
