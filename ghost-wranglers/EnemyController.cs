using Godot;
using System;

public partial class EnemyController : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.

	//Timer to keep track of how often enemy will move
	double timer = 0;

	//The initial direction the enemy moves
	Vector2 moveVector;

	//Tracks if this enemy is on its first process frame or not
	bool firstProcessFrame = true;
	
	protected TurnManager turnManager;
	 
	public override void _Ready()
	{
		turnManager = GetParent<TurnManager>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Gets the grid size from turn manager after it finishes its ready function to set the starting moveVector
		if (firstProcessFrame)
		{
			moveVector = new Vector2(turnManager.Grid[0]*2, 0);
			firstProcessFrame = false;
		}

		//Increments timer by the elapsed delta time
		timer += delta;

		if (timer >= 1)
		{
			timer -= 1;

			var space = turnManager.CollisionPlane.GetWorld2D().DirectSpaceState;

			var point = new PhysicsPointQueryParameters2D
			{
				Position = this.Position,
				CollideWithAreas = true,
				CollideWithBodies = false
			};
			
			//If the enemy is going to hit a wall, then it will bounce off
			if (space.IntersectPoint(point).Count == 0)
			{
				var predictedPoint = new PhysicsPointQueryParameters2D
				{
				Position = this.Position + new Vector2(Math.Sign(-moveVector.Y) * turnManager.Grid[0] * 2, Math.Sign(moveVector.X) * turnManager.Grid[1] * 2),
				CollideWithAreas = true,
				CollideWithBodies = false
				};
				//Check if enemy can bounce clockwise
				if (space.IntersectPoint(predictedPoint).Count > 0)
				{
					moveVector = new Vector2(Math.Sign(-moveVector.Y) * turnManager.Grid[0] * 2, Math.Sign(moveVector.X) * turnManager.Grid[1] * 2);
				}
				//Check if enemy can bounce counter clockwise
				else if (space.IntersectPoint(predictedPoint).Count > 0)
				{
					moveVector = new Vector2(Math.Sign(moveVector.Y) * turnManager.Grid[0] * 2, Math.Sign(-moveVector.X) * turnManager.Grid[1] * 2);
				}
				else //Reverses enemy movement direction, this only happens if the enemy hits a corner
				{
					//Reverses move direction
					moveVector *= -1;
				}
			}
			this.Translate(moveVector);
		}
	}
	

}
