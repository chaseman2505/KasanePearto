using Godot;
using System;
using System.Collections.Generic;

public partial class TurnManager : Node2D
{
	public enum GameState { win, loss, active};

	//The game state starts as active
	GameState gameState = GameState.active;  

	//A list of all characters that will actively take turns
	List<CharacterController> activeCharacters = new List<CharacterController>();
	//TileSet.TileSize;
	//float[] grid = [32.0f, 16.0f];
	Vector2 grid;
	TileMapLayer map;
	Area2D collisionPlane;

	//A list of all game objects that can be interacted with
	List<WorldObject> worldObjects = new List<WorldObject>();

	//A reference to the primary label UI
	Label labelUI;

	//A reference to the character indicator label UI
	Label characterIndicator;

	

	//The index of the character which is currently taking a turn
	//The initial value of this will indicate which character will take a turn first
	int currentCharacterIndex = 0;

	//How many interactions have happened (1 per object)
	int interactionCount = 0;

	//How many interactions must trigger to win the game
	const int interactionsGoal = 3;


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

	public Label CharacterIndicator
	{
		get { return characterIndicator; }
		set { characterIndicator = value; }
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

	public int InteractionCount
	{
		get { return interactionCount; }
		set { interactionCount = value; }
	}

	public int InteractionsGoal
	{
		get { return interactionsGoal; }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		labelUI = GetNode<Label>("InteractionText");
		characterIndicator = GetNode<Label>("CharacterIndicator");

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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Checks for character deaths
		for(int i = 0; i < activeCharacters.Count; i++)
		{
			if (activeCharacters[i].health == 0)
			{
				//Switches to another character if the current character died, removes character from the list, and makes them invisible
				if (i == currentCharacterIndex)
				{
					TurnSwitch();
				}
				activeCharacters[i].Visible = false;
				activeCharacters.Remove(activeCharacters[i]);
				i--;
			}
		}

		//Checks if every character is dead
		if (activeCharacters.Count == 0)
		{
			gameState = GameState.loss;
		}
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
					activeCharacters[currentCharacterIndex].GetChild<Sprite2D>(0).Texture = GD.Load<Texture2D>(activeCharacters[currentCharacterIndex].backRightSpriteFilePath);
					break;

				case Key.A:
					//activeCharacters[currentCharacterIndex].MoveCharacter(-32.0f, 0);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(-grid[0] / 2, -grid[1] / 2));
					activeCharacters[currentCharacterIndex].GetChild<Sprite2D>(0).Texture = GD.Load<Texture2D>(activeCharacters[currentCharacterIndex].backLeftSpriteFilePath);
					break;

				case Key.S:
					//activeCharacters[currentCharacterIndex].MoveCharacter(0, 50f);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(-grid[0] / 2, grid[1] / 2));
					activeCharacters[currentCharacterIndex].GetChild<Sprite2D>(0).Texture = GD.Load<Texture2D>(activeCharacters[currentCharacterIndex].frontLeftSpriteFilePath);
					break;

				case Key.D:
					//activeCharacters[currentCharacterIndex].MoveCharacter(50f, 0);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(grid[0] / 2, grid[1] / 2));
					activeCharacters[currentCharacterIndex].GetChild<Sprite2D>(0).Texture = GD.Load<Texture2D>(activeCharacters[currentCharacterIndex].frontRightSpriteFilePath);
					break;

				//Interacts with any nearby world objects
				case Key.E:
					foreach (WorldObject worldObject in worldObjects)
					{
						if (activeCharacters[currentCharacterIndex].GlobalPosition.DistanceTo(worldObject.GlobalPosition) <= 40)
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
				Position = activeCharacters[currentCharacterIndex].GlobalPosition,
				CollideWithAreas = true,
				//CollideWithBodies = false,
				//CollisionMask = uint.MaxValue 
			};

			//Moves character back to previous position if the new position is out of bounds
			if (space.IntersectPoint(point).Count == 0)
			{
				activeCharacters[currentCharacterIndex].GlobalPosition = activeCharacters[currentCharacterIndex].prevPos;
			}

			//Create a segment from previous character position to new position
			var seg = new SegmentShape2D();
			seg.A = activeCharacters[currentCharacterIndex].prevPos;
			seg.B = activeCharacters[currentCharacterIndex].GlobalPosition;

			//Build query
			var shapeQuery = new PhysicsShapeQueryParameters2D();
			shapeQuery.Shape = seg;
			shapeQuery.Transform = Transform2D.Identity;

			//Checks for all collisions along the segment
			var results2 = space.IntersectShape(shapeQuery);

			//Moves character back to previous position if the segment is colliding with any doors
			foreach (var result in results2)
			{
				if ((Node)result["collider"] is DoorObject)
				{
					if(((DoorObject)result["collider"]).isOpen == false)
					{
						activeCharacters[currentCharacterIndex].GlobalPosition = activeCharacters[currentCharacterIndex].prevPos;
					}
				}
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
				this.characterIndicator.Text = "---> Character 1\n       Character 2\n       Character 3\n       Character 4\n       Evidence: " + this.interactionCount;
				break;

			case 1:
				this.characterIndicator.Text = "       Character 1\n---> Character 2\n       Character 3\n       Character 4\n       Evidence: " + this.interactionCount;
				break;

			case 2:
				this.characterIndicator.Text = "       Character 1\n       Character 2\n---> Character 3\n       Character 4\n       Evidence: " + this.interactionCount;
				break;

			case 3:
				this.characterIndicator.Text = "       Character 1\n       Character 2\n       Character 3\n---> Character 4\n       Evidence: " + this.interactionCount;
				break;
		}

		activeCharacters[currentCharacterIndex].ReceiveTurn();
	}
}
