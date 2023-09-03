using Godot;

namespace DuckDuckKill.scripts;

public partial class Slime : CharacterBody2D
{
	public float Speed = 100.0f;
	public float Health = 100.0f;

	private AnimatedSprite2D _aSprite;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		_aSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_aSprite.Animation = "idle";
		_aSprite.Play();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		velocity.X = Speed;

		Velocity = velocity;
		MoveAndSlide();
		
		HandleAnimation();
		
		// Change move direction
		if (Velocity.X == 0.0f)
		{
			Speed = -Speed;
		}
		
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			if (((Node) collision.GetCollider()).Name.ToString().Contains("Bullet"))
			{
				Health -= 10.0f;
				if (Health <= 0.0f)
				{
					this.QueueFree();
				}
			}
		}
	}

	private void HandleAnimation()
	{
		if (Velocity.X == 0.0f)
		{
			
			_aSprite.Play("idle");
		}
		else
		{
			_aSprite.FlipH = Velocity.X < 0;
			_aSprite.Play("walk");
		}
	}
}