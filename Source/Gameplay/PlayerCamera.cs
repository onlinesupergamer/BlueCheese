using Godot;
using System;

public partial class PlayerCamera : Camera3D
{
	[Export] public Node3D m_Car;
	[Export] float HeightOffset;
	[Export] float DistanceOffset;


	
	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        Vector3 TargetPos = m_Car.GlobalPosition + m_Car.Basis.Z * DistanceOffset + m_Car.Basis.Y * HeightOffset;


		GlobalPosition = GlobalPosition.Lerp(TargetPos, (float)delta * 10.0f);
		GlobalBasis = GlobalBasis.Slerp(m_Car.Basis, (float)delta * 4.0f);
    }

}
