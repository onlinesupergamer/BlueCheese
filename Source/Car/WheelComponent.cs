using Godot;
using System;

public partial class WheelComponent : RayCast3D
{
	[Export] RigidBody3D CarRb;
	[Export] float MaxWheelDistance;
	[Export] float StiffnessValue;
	[Export] float RestLength;
	[Export] float DampingValue;
	[Export] float OverExtend;
	[Export] float WheelRadius;
	[Export] public bool bIsSteering;


	float CurrentLength;
	float PrevousLength;
	float Force;



	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
		UpdateSuspension(delta);
		Friction();

    }

	void UpdateSuspension(double delta)
	{
		Vector3 Start = GlobalPosition;
		Vector3 Offset = Start - CarRb.GlobalPosition;

		if(IsColliding())
		{
			CurrentLength = Start.DistanceTo(GetCollisionPoint());
			float Velocity = (PrevousLength - CurrentLength) / (float) delta;
			Force = StiffnessValue * (RestLength - CurrentLength) + DampingValue * Velocity;

			CarRb.ApplyForce(GetCollisionNormal() * Force, Offset);

			PrevousLength = CurrentLength;

		}
	}

	void TestSuspension()
	{
		if(IsColliding())
		{	
			
			
		}
	}

	void Friction()
	{
		if(IsColliding())
		{
			Vector3 SteeringSideDir = GlobalBasis.X;
			Vector3 TireVelocity = GetPointVelocity(GlobalPosition);
			float SteeringXVel = SteeringSideDir.Dot(TireVelocity);
			float XTraction = 1.0f;
			float Gravity = 9.81f;
			Vector3 XForce = -SteeringSideDir * SteeringXVel * XTraction * ((CarRb.Mass * Gravity) / 4.0f);
			Vector3 ForcePosition = GlobalPosition - CarRb.GlobalPosition;

			CarRb.ApplyForce(XForce, ForcePosition);

		}
	}

	public Vector3 GetPointVelocity(Vector3 m_Point)
	{
		return CarRb.LinearVelocity + CarRb.AngularVelocity.Cross(m_Point - CarRb.GlobalPosition);
	}

}
