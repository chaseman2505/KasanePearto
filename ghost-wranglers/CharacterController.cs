using Godot;
using System;
using System.Diagnostics;

public partial class CharacterController : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

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
