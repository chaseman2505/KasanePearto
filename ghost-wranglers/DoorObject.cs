using Godot;
using System;

public partial class DoorObject : WorldObject
{
	//If the door is open
	public bool isOpen = false;

	//What the interaction text will display
	[Export]
	protected string interactionText = "This is a door. Like most objects, I can interact with it by pressing E.\nPress Esc to Hide/Show text";

	//What the interaction text will display if the door is locked
	[Export]
	protected string interactionText2 = "This case isn't solved yet. I need to collect enough evidence before I leave.\nPress Esc to Hide/Show text";
	
	//If the door requires all evidence to be collected in order to open
	[Export]
	private bool requiresEvidence = false;

	public override void _Ready()
	{
		turnManager = GetParent<TurnManager>();
	}

	
	public override void TriggerInteraction()
	{
		//turnManager.LabelUI.Text = turnManager.ActiveCharacters[turnManager.CurrentCharacterIndex].Name + " is interacting with " + this.Name;
		turnManager.LabelUI.Visible = true;

		//If the door doesn't require evidence to open or if enough enough has been collected, then open/close
		if (!requiresEvidence || turnManager.InteractionCount >= turnManager.InteractionsGoal)
        {
			//Changes text of label UI
			turnManager.LabelUI.Text = interactionText;

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
        else
        {
            //Changes text of label UI
			turnManager.LabelUI.Text = interactionText2;
        }
	}
}
