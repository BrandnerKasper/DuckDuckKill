using Godot;

namespace DuckDuckKill.scripts;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 200.0f;
	
	public float MaxVelocity = 300.0f;
	public float Acceleration = 15.0f;
	public float Deceleration = 25.0f;
	[Export]
	public float JumpVelocity = -400.0f;
	
	private AnimatedSprite2D _aSprite;
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		_aSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		//GD.Print(Engine.GetFramesPerSecond());
		Vector2 velocity = Velocity;
;
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = Jump();

		velocity.X = Move();

		Velocity = velocity;
		MoveAndSlide();
	}

	private float Move()
	{
		float velocityX = 0.0f;
		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("left", "right", "down", "up").Normalized();
		if (direction != Vector2.Zero)
		{
			_aSprite.FlipH = false;
			velocityX = Mathf.MoveToward(Velocity.X, (direction.X * Speed), Acceleration);
			_aSprite.FlipH = velocityX > 0;
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
}
