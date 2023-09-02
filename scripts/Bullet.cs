using System;
using Godot;

public partial class Bullet : CharacterBody2D
{
	private float _lifeTime = 2.0f;
	private int _speed = 500;

	public override void _Process(double delta)
	{
		_lifeTime -= (float)delta;
		if (_lifeTime <= 0)
		{
			GetParent().QueueFree();
		}
	}

	public override void _PhysicsProcess(double delta)  // Sticking with float for Godot's usual _PhysicsProcess
	{
		// Define falloff rate per second
		float falloffRate = 0.01f; 

		// Compute the falloff factor for this frame
		float falloffFactor = (float)Math.Exp(-falloffRate * delta);
		
		Velocity *= falloffFactor;

		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			Velocity = Velocity.Bounce(collision.GetNormal());
		}
	}

	public void Start(float Rotation)
	{
		//GlobalPosition = Pos;
		this.Rotation = Rotation;
		// move in the direction of the _rotation
		Vector2 velocity = new Vector2(0, 0);
		velocity.X = Mathf.Cos(Rotation);
		velocity.Y = Mathf.Sin(Rotation);
		Velocity = velocity.Normalized() * _speed;
	}


	private void OnVisibilityNotifier2DScreenExited()
	{
		GD.Print("Bullet exited the screen");
		// Deletes the bullet when it exits the screen.
		QueueFree();
	}
}
