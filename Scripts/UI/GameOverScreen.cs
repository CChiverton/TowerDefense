using Godot;
using System;

public partial class GameOverScreen : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void OnRestartButtonPressed()
	{
		GetTree().ReloadCurrentScene();
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
