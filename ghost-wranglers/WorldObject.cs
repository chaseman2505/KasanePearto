using Godot;
using System;

public partial class WorldObject : CharacterBody2D
{
	//A reference to the turn manager
	protected TurnManager turnManager;
	
	float[] grid = [32.0f, 8.0f];
		
	public override void _Ready()
	{
		turnManager = GetParent<TurnManager>();
		Vector2 gridBound = new Vector2(0,0);
		gridBound[0] = (GlobalPosition[0] % grid[0]) - 2;
		gridBound[1] = (GlobalPosition[1] % grid[1]) ;
		Translate(gridBound);
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
