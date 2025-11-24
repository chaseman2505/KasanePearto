using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class BoxObject : WorldObject
{
	
	//What the interaction text will display
	[Export]
	protected string interactionText = "This is a box. Doesn't seem like there's anything to do with this yet.\nPress Esc to Hide/Show Text";

	public override void TriggerInteraction()
	{
		base.TriggerInteraction();
	}
}
