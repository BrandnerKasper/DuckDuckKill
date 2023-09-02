using System;
using Godot;

public partial class Bullet : CharacterBody2D
{
	private int _speed = 500;

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
			GD.Print(collision.GetCollider());
			Velocity = Velocity.Bounce(collision.GetNormal()) * 0.2f;
			if (collision.GetCollider().HasMethod("Hit"))
			{
				collision.GetCollider().Call("Hit");
			}
		}
	}

	public void Start(Vector2 Pos, float Rotation)
	{
		GlobalPosition = Pos;
		this.Rotation = Rotation;
		// move in the direction of the _rotation
		Vector2 velocity = new Vector2(0, 0);
		velocity.X = Mathf.Cos(Rotation);
		velocity.Y = Mathf.Sin(Rotation);
		Velocity = velocity.Normalized() * _speed;
		
		//Velocity = new Vector2(-Mathf.Cos(Rotation), -Mathf.Sin(Rotation)).Normalized() * _speed;
	}


	private void OnVisibilityNotifier2DScreenExited()
	{
		// Deletes the bullet when it exits the screen.
		QueueFree();
	}
}
