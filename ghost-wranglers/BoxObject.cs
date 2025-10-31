using Godot;
using System;

public partial class BoxObject : WorldObject
{
	//
	public override void TriggerInteraction()
	{
		//turnManager.LabelUI.Text = turnManager.ActiveCharacters[turnManager.CurrentCharacterIndex].Name + " is doing something with " + this.Name;
		turnManager.LabelUI.Text = "This is a box. Doesn't seem like there's anything to do with this yet.\nPress Esc to Toggle Text On/Off";
		turnManager.LabelUI.Visible = true;
	}
}
