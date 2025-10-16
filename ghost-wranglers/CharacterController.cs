using Godot;
using System;
using System.Diagnostics;

public partial class CharacterController : Node2D
{
	float[] grid = [8.0f, 4.0f];
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2 gridBound = new Vector2(0,0);
		gridBound[0] = Position[0] - (Position[0] % grid[0]) + grid[0];
		gridBound[1] = Position[1] - (Position[1] % grid[1]) + grid[1];
		Translate(gridBound);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	//Called when this node's turn starts
	public void ReceiveTurn()
	{
		//this.Rotate(1);
		GD.Print(this.Name + " Turn Started");
	}
}
