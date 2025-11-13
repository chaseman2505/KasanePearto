using Godot;
using System;
using System.Diagnostics;

public partial class Collisionscript : Area2D
{
	public bool is_colliding(Node body){
		return OverlapsBody(body);
	}
}
