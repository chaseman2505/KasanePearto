using Godot;
using System;

public partial class WorldObject : CharacterBody2D
{
	//A reference to the turn manager
	protected TurnManager turnManager;

	public override void _Ready()
	{
		turnManager = GetParent<TurnManager>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	//
	public virtual void TriggerInteraction()
	{
		//turnManager.LabelUI.Text = turnManager.ActiveCharacters[turnManager.CurrentCharacterIndex].Name + " Is Interacting With " + this.Name;
		turnManager.LabelUI.Text = "There's a bloodstain here, why is it so far from the body?\nPress Esc to Toggle Text On/Off";
		turnManager.LabelUI.Visible = true;
	}
}
