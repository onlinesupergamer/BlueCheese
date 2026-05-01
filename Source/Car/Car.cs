using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;



public partial class Car : RigidBody3D
{
	[Export] WheelComponent[] Wheels = new WheelComponent[4];

	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}
}
