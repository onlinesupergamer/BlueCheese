using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;
using System.ComponentModel;



public partial class Car : RigidBody3D
{
	[Export] WheelComponent[] Wheels = new WheelComponent[4];
	[Export] float EngineTorque = 300.0f;
	[Export] float TireTurnSpeed = 2.0f;
	[Export] float TireTurnMax = 25.0f;



	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        Drive();
		Steer();
    }

	void Drive()
	{
		float AccelInput = Input.GetActionStrength("Accelerate");
		float BrakeInput = Input.GetActionStrength("Brake");

		for(int i = 0; i < Wheels.Length; i++)
		{
			if(Wheels[i].IsColliding())
			{
				if(AccelInput > 0.0f)
				{
					ApplyForce(-Wheels[i].GlobalBasis.Z * (EngineTorque * AccelInput), GlobalPosition - Wheels[i].GlobalPosition);
					
				}
				if(BrakeInput > 0.0f)
				{
					ApplyForce(Wheels[i].GlobalBasis.Z * (EngineTorque * BrakeInput), GlobalPosition - Wheels[i].GlobalPosition);
					
				}		
			}
		}
	}

	void Steer()
	{
		float RightInput = -Input.GetActionStrength("SteerRight");
		float LeftInput = -Input.GetActionStrength("SteerLeft");
		float SteeringInput = RightInput - LeftInput;

		for (int i = 0; i < Wheels.Length; i++)
		{
			if(Wheels[i].bIsSteering)
			{
				Vector3 NewRot = new Vector3(0, SteeringInput * TireTurnMax, 0);
				Wheels[i].RotationDegrees = NewRot;
			}
		}
	}
}
