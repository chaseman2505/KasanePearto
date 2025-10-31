using Godot;
using System;

public partial class BoxObject : WorldObject
{
	//
	public override void TriggerInteraction()
	{
		GD.Print(turnManager.ActiveCharacters[turnManager.CurrentCharacterIndex] + " is doing something with" + this.Name);
		turnManager.LabelUI.Text = turnManager.ActiveCharacters[turnManager.CurrentCharacterIndex].Name + " is doing something with " + this.Name;
		turnManager.LabelUI.Visible = true;
	}
}
