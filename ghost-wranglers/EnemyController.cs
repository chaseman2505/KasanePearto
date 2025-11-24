using Godot;
using System;

public partial class EnemyController : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.

	float[] grid = [32.0f, 8.0f];

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
		Vector2 gridBound = new Vector2(0,0);
		gridBound[0] = (GlobalPosition[0] % grid[0]) - 2;
		gridBound[1] = (GlobalPosition[1] % grid[1]);
		Translate(gridBound);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Gets the grid size from turn manager after it finishes its ready function to set the starting moveVector
		if (firstProcessFrame)
		{
			moveVector = new Vector2(-turnManager.Grid[0], 0);
			firstProcessFrame = false;
		}

		//Checks if the enemy ever comes close enough to a character and kills them if so
		foreach (CharacterController character in turnManager.ActiveCharacters)
		{
			if (this.GlobalPosition.DistanceTo(character.GlobalPosition) <= 20)
			{
				character.health = 0;
			}
		}

		//Increments timer by the elapsed delta time
		timer += delta;

		if (timer >= 1)
		{
			//Resets timer
			timer -= 1;

			//The space where the enemy can move
			var space = turnManager.CollisionPlane.GetWorld2D().DirectSpaceState;

			//The point where the enemy is about to move
			var point = new PhysicsPointQueryParameters2D
			{
				Position = this.Position + moveVector,
				CollideWithAreas = true,
				CollideWithBodies = false
			};
			
			//If the point the enemy is about to move is out of bounds then it will change the move vector to bounce
			if (space.IntersectPoint(point).Count == 0)
			{
				//Location the enemy can bounce to if it bounces clockwise
				var predictedPoint1 = new PhysicsPointQueryParameters2D
				{
					Position = this.Position + new Vector2(Math.Sign(-moveVector.Y) * turnManager.Grid[0], Math.Sign(moveVector.X) * turnManager.Grid[1]),
					CollideWithAreas = true,
					CollideWithBodies = false
				};

				//Location the enemy can bounce to if it bounces counter clockwise
				var predictedPoint2 = new PhysicsPointQueryParameters2D
				{
					Position = this.Position + new Vector2(Math.Sign(moveVector.Y) * turnManager.Grid[0], Math.Sign(-moveVector.X) * turnManager.Grid[1]),
					CollideWithAreas = true,
					CollideWithBodies = false
				};

				//Check if enemy can bounce clockwise
				if (space.IntersectPoint(predictedPoint1).Count > 0)
				{
					moveVector = new Vector2(Math.Sign(-moveVector.Y) * turnManager.Grid[0], Math.Sign(moveVector.X) * turnManager.Grid[1]);
				}
				//Check if enemy can bounce counter clockwise
				else if (space.IntersectPoint(predictedPoint2).Count > 0)
				{
					moveVector = new Vector2(Math.Sign(moveVector.Y) * turnManager.Grid[0], Math.Sign(-moveVector.X) * turnManager.Grid[1]);
				}
				//Reverses enemy movement direction, this only happens if the enemy hits a corner
				else
				{
					moveVector *= -1;
				}
			}
			this.Translate(moveVector);
		}
	}
	

}
