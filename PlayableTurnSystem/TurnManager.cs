using Godot;
using System;
using System.Collections.Generic;

public partial class TurnManager : Node
{
	//A list of all characters that will actively take turns
	List<CharacterController> activeCharacters = new List<CharacterController>();

	//The index of the character which is currently taking a turn
	//The initial value of this will indicate which character will take a turn first
	int currentCharacterIndex = 0;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("WASD to move, click to switch turns");

		//Get all children of CharacterManager as Godot array
		Godot.Collections.Array<Node> childrenArray = GetChildren();

		//Populate activeCharacter list with all active character nodes
		foreach (Node child in childrenArray)
		{
			//Checks if the child has a CharacterController before adding to the list
			if (child is CharacterController)
			{
				activeCharacters.Add((CharacterController)child);
			}
		}

		activeCharacters[currentCharacterIndex].ReceiveTurn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	//Character input is processed here
	public override void _UnhandledInput(InputEvent @event)
	{
		//If the mouse is clicked, switch which character is currently taking a turn
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			this.TurnSwitch();
		}

		//If WASD is released, moves the current character a certain amount
		if (@event is InputEventKey keyEvent && !keyEvent.Pressed)
		{
			switch (keyEvent.Keycode)
			{
				case Key.W:
					//activeCharacters[currentCharacterIndex].MoveCharacter(32.0f, -32.0f);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(32.0f, -32.0f));
					break;

				case Key.A:
					//activeCharacters[currentCharacterIndex].MoveCharacter(-32.0f, 0);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(-32.0f, -32.0f));
					break;

				case Key.S:
					//activeCharacters[currentCharacterIndex].MoveCharacter(0, 50f);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(-32.0f, 32.0f));
					break;

				case Key.D:
					//activeCharacters[currentCharacterIndex].MoveCharacter(50f, 0);
					activeCharacters[currentCharacterIndex].Translate(new Vector2(32.0f, 32.0f));
					break;
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

		activeCharacters[currentCharacterIndex].ReceiveTurn();
	}
}
