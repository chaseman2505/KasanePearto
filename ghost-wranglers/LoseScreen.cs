using Godot;
using System;

public partial class LoseScreen : Sprite2D
{
	public override void _Ready()
	{
		Button btn = GetNode<Button>("QuitButton");
		btn.Pressed += OnQuitButtonPressed;

		Button btn2 = GetNode<Button>("RestartButton");
		btn2.Pressed += OnRestartButtonPressed;
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
	private void OnRestartButtonPressed()
	{
		GetTree().ReloadCurrentScene();
	}
}
