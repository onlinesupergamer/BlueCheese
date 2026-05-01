using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;



public partial class Car : RigidBody3D
{
	[Export] WheelComponent[] Wheels = new WheelComponent[4];
	[Export] float EngineTorque = 300.0f;


	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        Drive();
    }

	void Drive()
	{
		float AccelInput = Input.GetActionStrength("Accelerate");


		for(int i = 0; i < Wheels.Length; i++)
		{
			if(Wheels[i].IsColliding())
			{
				ApplyForce(-Wheels[i].Basis.Z * (EngineTorque * AccelInput), Wheels[i].GetCollisionPoint());
				
			}
		}
	}

}
