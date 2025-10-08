using Godot;
using System;

public partial class IsoGrid_Move : CharacterBody2D{
	int[]curPos = [0, 0];
	
	public override void _Input(InputEvent @event){
		
		if(@event is InputEventKey keyEvent && keyEvent.Pressed){
			
			/*switch for movement. broken down by case to prevent diagonals
			this will be changed later to acomadate diagonals, but to reduce complexity for now
			It will not include diagonals*/
			switch(keyEvent.Keycode){
				//case Key.RIGHT:
				case Key.D:
					curPos[0] += 32;
					curPos[1] += 32;
					break;
				//case Key.UP:
				case Key.W:
					curPos[0] += 32;
					curPos[1] -= 32;
					break;
				//case Key.LEFT:
				case Key.A:
					curPos[0] -= 32;
					curPos[1] -= 32;
					break;
				//case Key.DOWN:
				case Key.S:
					curPos[0] -= 32;
					curPos[1] += 32;
					break;
			}
			
			//catch for going off grid
			if(curPos[0] < 0){
				curPos[0] = 0;
			}
			if(curPos[0] < 0){
				curPos[0] = 0;
			}
			
			Position = new Vector2(curPos[0], curPos[1]);
		}
	}
}
