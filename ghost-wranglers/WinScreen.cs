using Godot;
using System;

public partial class WinScreen : Sprite2D
{
	TurnManager turnManager;
	Label winText;
	public override void _Ready()
	{
		Button btn = GetNode<Button>("QuitButton");
		btn.Pressed += OnQuitButtonPressed;

		Button btn2 = GetNode<Button>("RestartButton");
		btn2.Pressed += OnRestartButtonPressed;

		turnManager = GetParent<TurnManager>();
		winText = GetNode<Label>("WinText");
	}

	public override void _Process(double delta)
	{
		winText.Text = "You survived and solved the case with " + turnManager.Points + " party members left alive";
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
