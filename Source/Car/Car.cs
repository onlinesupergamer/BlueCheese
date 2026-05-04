using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;
using System.ComponentModel;



public partial class Car : RigidBody3D
{
	[Export] WheelComponent[] Wheels = new WheelComponent[4];
	[Export] Curve EngineCurve = new Curve();
	[Export] Curve SteeringCurve = new Curve(); //This might not be needed
	[Export] float EngineTorque = 300.0f;
	[Export] float MaxSpeed;
	[Export] float TireTurnMax = 25.0f;
	[Export] float GravityValue;



	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
		Gravity(delta);
        Drive();
		Steer();
    }

	void Drive()
	{
		float AccelInput = Input.GetActionStrength("Accelerate");
		float BrakeInput = Input.GetActionStrength("Brake");

		float CurrentSpeed = Math.Clamp(GetCurrentSpeed() / MaxSpeed, 0.0f, 1.0f);
		float TotalTorque = EngineCurve.SampleBaked(CurrentSpeed) * EngineTorque;
		


		for(int i = 0; i < Wheels.Length; i++)
		{
			if(Wheels[i].IsColliding())
			{
				if(AccelInput > 0.0f)
				{
					ApplyForce(-Wheels[i].GlobalBasis.Z *  (TotalTorque * AccelInput), GlobalPosition - Wheels[i].GlobalPosition);
					
				}
				if(BrakeInput > 0.0f)
				{
					ApplyForce(Wheels[i].GlobalBasis.Z * (TotalTorque * BrakeInput), GlobalPosition - Wheels[i].GlobalPosition);
					
				}		
			}
		}
	}

	float GetCurrentSpeed()
	{
		return LinearVelocity.Dot(-GlobalBasis.Z);
	}

	void Steer()
	{
		float RightInput = -Input.GetActionStrength("SteerRight");
		float LeftInput = -Input.GetActionStrength("SteerLeft");
		float SteeringInput = RightInput - LeftInput;

		float CurrentSpeed = Math.Clamp(GetCurrentSpeed() / MaxSpeed, 0.0f, 1.0f);
		float TotalSteering = SteeringCurve.SampleBaked(CurrentSpeed) * TireTurnMax;

		for (int i = 0; i < Wheels.Length; i++)
		{
			if(Wheels[i].bIsSteering)
			{
				Vector3 NewRot = new Vector3(0, SteeringInput * TotalSteering, 0);
				Wheels[i].RotationDegrees = NewRot;
			}
		}
	}

	void Gravity(double delta)
	{
		Vector3 GravityForce = new Vector3(0, 0, 0);

		if(Wheels[0].bIsGrounded || Wheels[1].bIsGrounded || Wheels[2].bIsGrounded || Wheels[3].bIsGrounded)
		{
			//Consider this grounded
			GravityForce = -GlobalBasis.Y * GravityValue;

		}
		
		else
		{
			GravityForce = new Vector3(0, -1, 0) * GravityValue;
		}

		ApplyCentralForce(GravityForce * (float)delta);
	}
}
