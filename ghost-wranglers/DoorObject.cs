using Godot;
using System;

public partial class DoorObject : WorldObject
{
	//If the door is open
	bool isOpen = false;

	public override void TriggerInteraction()
	{
		//turnManager.LabelUI.Text = turnManager.ActiveCharacters[turnManager.CurrentCharacterIndex].Name + " is interacting with " + this.Name;
		turnManager.LabelUI.Text = "This is a door. Like most objects, I can interact with it by pressing E.\nPress Esc to Toggle Text On/Off";
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
