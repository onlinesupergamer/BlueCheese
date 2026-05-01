using Godot;
using System;

public partial class WheelComponent : RayCast3D
{
	[Export] RigidBody3D CarRb;
	[Export] float MaxWheelDistance;
	[Export] float StiffnessValue;
	[Export] float RestLength;
	[Export] float DampingValue;


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
		Vector3 Start = GlobalPosition;

		if(IsColliding())
		{
			CurrentLength = Start.DistanceTo(GetCollisionPoint());
			float Velocity = (PrevousLength - CurrentLength) / (float)delta;
			Force = StiffnessValue * (RestLength - CurrentLength) + DampingValue * Velocity;

			CarRb.ApplyForce(Basis.Y * Force, Position);

			PrevousLength = CurrentLength;

		}
    }

}
