using Godot;
using System;

public partial class ShopButtons : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void OnButtonDown()
	{
		GD.Print("Button has been pressed");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
