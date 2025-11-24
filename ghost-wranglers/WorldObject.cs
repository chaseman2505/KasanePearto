using Godot;
using System;

public partial class WorldObject : CharacterBody2D
{
	//A reference to the turn manager
	protected TurnManager turnManager;

	//If the object has been interacted with at least once
	protected bool firstInteraction = false;

	//What the interaction text will display
	[Export]
	protected string interactionText = "There's a bloodstain here, why is it so far from the body?\nPress Esc to Hide/Show text";

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
		this.AddInteraction();
		turnManager.LabelUI.Text = interactionText;
		turnManager.LabelUI.Visible = true;
	}


	//Adds to the interaction count the first time this object is interacted with
	public virtual void AddInteraction()
	{
		if(firstInteraction == false)
		{
			firstInteraction = true;
			turnManager.InteractionCount++;
			
			//Edits the last character of the label
			GD.Print(turnManager.InteractionCount);
			turnManager.CharacterIndicator.Text = turnManager.CharacterIndicator.Text.Substring(0, turnManager.CharacterIndicator.Text.Length - 1) + turnManager.InteractionCount;
		}
	}
}
