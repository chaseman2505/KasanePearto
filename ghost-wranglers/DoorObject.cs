using Godot;
using System;

public partial class DoorObject : WorldObject
{
	//If the door is open
	public bool isOpen = false;

	//What the interaction text will display
	[Export]
	protected string interactionText = "This is a door. Like most objects, I can interact with it by pressing E.\nPress Esc to Hide/Show text";

	public override void _Ready()
	{
		turnManager = GetParent<TurnManager>();
	}

	
	public override void TriggerInteraction()
	{
		//turnManager.LabelUI.Text = turnManager.ActiveCharacters[turnManager.CurrentCharacterIndex].Name + " is interacting with " + this.Name;
		turnManager.LabelUI.Text = interactionText;
		turnManager.LabelUI.Visible = true;
		
		//Changes the state of the doorObject
		isOpen = !isOpen;

		//Changes the texture of the sprite (the sprite2D must be the first child of the door object)
		if (isOpen)
		{
			GetChild<Sprite2D>(0).Texture = GD.Load<Texture2D>("res://door right.png");
		}
		else
		{
			GetChild<Sprite2D>(0).Texture = GD.Load<Texture2D>("res://door left.png");
		}
	}
}
