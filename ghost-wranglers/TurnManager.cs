using Godot;
using System;
using System.Collections.Generic;

public partial class TurnManager : Node2D
{
	//A list of all characters that will actively take turns
	List<CharacterController> activeCharacters = new List<CharacterController>();
	//TileSet.TileSize;
	//float[] grid = [32.0f, 16.0f];
	Vector2 grid;
	TileMapLayer map;
	Area2D collisionPlane;

	//A list of all game objects that can be interacted with
	List<WorldObject> worldObjects = new List<WorldObject>();

	

	//The index of the character which is currently taking a turn
	//The initial value of this will indicate which character will take a turn first
	int currentCharacterIndex = 0;

	//A reference to the primary label UI
	Label labelUI;



	public List<CharacterController> ActiveCharacters
	{
		get { return activeCharacters; }
		set { activeCharacters = value; }
	}
	
	public int CurrentCharacterIndex
	{
		get { return currentCharacterIndex; }
		set { currentCharacterIndex = value; }
	}
	
	public List<WorldObject> WorldObjects
	{
		get { return worldObjects; }
		set { worldObjects = value; }
	}
	
	public Label LabelUI
	{
		get { return labelUI; }
		set { labelUI = value; }
	}

	public Vector2 Grid
	{
		get { return grid; }
		set { grid = value; }
	}

	public Area2D CollisionPlane
	{
		get { return collisionPlane; }
		set { collisionPlane = value; }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		labelUI = GetNode<Label>("Label");

		//Get all children of CharacterManager as Godot array
		Godot.Collections.Array<Node> childrenArray = GetChildren();
		
		Node walls = GetNode("walls");
		collisionPlane = (Area2D)walls.GetNode("Area2DPlane");
		

		//Populate activeCharacter list with all active character nodes
		//and populate worldObjects with all world objects
		foreach (Node child in childrenArray)
		{
			//Checks if the child has a CharacterController script before adding to the activeCharacters list
			if (child is CharacterController)
			{
				activeCharacters.Add((CharacterController)child);
			}
			else if (child is TileMapLayer)
			{
				map = (TileMapLayer)child;
			}
			//Checks if the child has a WorldObject script before adding to the worldObjects list
			else if (child is WorldObject)
			{
				worldObjects.Add((WorldObject)child);
			}
			//else if(child is 	Area2D){
				//collisionPlane = (Area2D)child;
			//}
		}

		//Starts the turn for the first active character
		activeCharacters[currentCharacterIndex].ReceiveTurn();
		
		grid[0] = map.TileSet.TileSize.X;
		grid[1] = map.TileSet.TileSize.Y;
		GD.Print(grid[0]);
		GD.Print(grid[1]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	//Character input is processed here
	public override void _UnhandledInput(InputEvent @event)
	{
		Vector2 revert = new Vector2(0, 0);
		//If the mouse is clicked, switch which character is currently taking a turn
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			this.TurnSwitch();
		}

		//If WASD is released, moves the current character a certain amount
		if (@event is InputEventKey keyEvent && !keyEvent.Pressed)
		{
			//Tracks the previous position of the character before the character moves
			activeCharacters[currentCharacterIndex].prevPos = activeCharacters[currentCharacterIndex].Position;

			switch (keyEvent.Keycode)
			{
				case Key.W:
					activeCharacters[currentCharacterIndex].Translate(new Vector2(grid[0] / 2, -grid[1] / 2));
					revert = new Vector2(grid[0], -grid[1]);
					break;

				case Key.A:
					//activeCharacters[currentCharacterIndex].MoveCharacter(-32.0f, 0);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(-grid[0] / 2, -grid[1] / 2));
					revert = new Vector2(-grid[0], -grid[1]);
					break;

				case Key.S:
					//activeCharacters[currentCharacterIndex].MoveCharacter(0, 50f);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(-grid[0] / 2, grid[1] / 2));
					revert = new Vector2(-grid[0], grid[1]);
					break;

				case Key.D:
					//activeCharacters[currentCharacterIndex].MoveCharacter(50f, 0);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(grid[0] / 2, grid[1] / 2));
					revert = new Vector2(grid[0], grid[1]);
					break;

				//Interacts with any nearby world objects
				case Key.E:
					foreach (WorldObject worldObject in worldObjects)
					{
						if (activeCharacters[currentCharacterIndex].GlobalPosition.DistanceTo(worldObject.GlobalPosition) <= 50)
						{
							worldObject.TriggerInteraction();
						}
					}
					break;

				//Toggles label visibility
				case Key.Escape:
					labelUI.Visible = !labelUI.Visible;
					break;
			}

			//The space where the character can move
			var space = collisionPlane.GetWorld2D().DirectSpaceState;

			//The point the character is currently at
			var point = new PhysicsPointQueryParameters2D
			{
				Position = activeCharacters[currentCharacterIndex].Position,
				CollideWithAreas = true,
				CollideWithBodies = false
			};

			//Moves character back to previous position if they are out of bounds
			if (space.IntersectPoint(point).Count == 0)
			{
				activeCharacters[currentCharacterIndex].Position = activeCharacters[currentCharacterIndex].prevPos;
			}
		}

	}

	//Called when the turn is being switched
	public void TurnSwitch()
	{
		currentCharacterIndex++;
		if (currentCharacterIndex >= activeCharacters.Count)
		{
			currentCharacterIndex = 0;
		}

		switch(currentCharacterIndex)
		{
			case 0:
				this.GetNode<Label>("CharacterIndicator").Text = "---> Character 1\n       Character 2\n       Character 3\n       Character 4";
				break;

			case 1:
				this.GetNode<Label>("CharacterIndicator").Text = "       Character 1\n---> Character 2\n       Character 3\n       Character 4";
				break;

			case 2:
				this.GetNode<Label>("CharacterIndicator").Text = "       Character 1\n       Character 2\n---> Character 3\n       Character 4";
				break;

			case 3:
				this.GetNode<Label>("CharacterIndicator").Text = "       Character 1\n       Character 2\n       Character 3\n---> Character 4";
				break;
		}

		activeCharacters[currentCharacterIndex].ReceiveTurn();
	}
}
