using Godot;

namespace DuckDuckKill.scripts;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 200.0f;
	[Export]
	public float Acceleration = 15.0f;
	[Export]
	public float Deceleration = 25.0f;
	[Export]
	public float JumpVelocity = -300.0f;

	[Export] public float PushForce = 200.0f;
	
	private AnimatedSprite2D _aSprite;
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		_aSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_aSprite.Animation = "idle";
		_aSprite.Play();
	}

	public override void _PhysicsProcess(double delta)
	{
		//GD.Print(Engine.GetFramesPerSecond());
		Vector2 velocity = Velocity;
		
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = Jump();

		velocity.X = Move();
		
		
		Velocity = velocity;
		HandleAnimation();
		if (MoveAndSlide())
			HandleCollision();	
		// MoveAndSlide();
	}

	private float Move()
	{
		float velocityX = 0.0f;
		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("left", "right", "down", "up").Normalized();
		if (direction != Vector2.Zero)
		{
			velocityX = Mathf.MoveToward(Velocity.X, (direction.X * Speed), Acceleration);
		}
		else
		{
			velocityX = Mathf.MoveToward(Velocity.X, 0, Deceleration);
		}

		return velocityX;
	}

	private float Jump()
	{
		return JumpVelocity;
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

	private void HandleCollision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (collision.GetCollider() is RigidBody2D)
			{
				Node2D collidedNode = collision.GetCollider() as Node2D;
				if (collidedNode != null)
				{
					GD.Print($"Collided with: {collidedNode.Name}");
				}
				RigidBody2D Col = collision.GetCollider() as RigidBody2D;
				Col.ApplyImpulse(collision.GetNormal() * - PushForce);
			}
		}
	}
	
}
