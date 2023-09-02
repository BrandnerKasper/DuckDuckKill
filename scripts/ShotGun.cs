using Godot;

namespace DuckDuckKill.scripts;

public partial class ShotGun : Node2D
{
	[Export] 
	public float WaitTimeBetweenShots = 0.5f;
	[Export] 
	public float Recoil = 500.0f;
	
	private AnimatedSprite2D _animatedSprite2D;
	private Timer _timerBetweenShots;
	private bool _isShooting = false;

	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite2D.AnimationLooped += () => _animatedSprite2D.Play("default");
		_timerBetweenShots = GetNode<Timer>("TimerBetweenShots");
		_timerBetweenShots.Timeout += () => _isShooting = false;
	}


	[Export] public PackedScene BulletScene { get; set; }
	public override void _Process(double delta)
	{
		UpdateRotation();
		return;

		void UpdateRotation()
		{
			Vector2 mousePos = GetGlobalMousePosition();
			Vector2 playerPos = GlobalPosition;

			float deltaX = mousePos.X - playerPos.X;
			float deltaY = mousePos.Y - playerPos.Y;

			float angle = Mathf.Atan2(deltaY, deltaX);
			Rotation = angle;
		}
	}

	public override void _Input(InputEvent @event)
	{
		// if mouse left button is pressed
		if (!_isShooting && @event.IsActionPressed("mouse_left"))
		{
			SpawnBullet();
			AddScreenShake();
			_animatedSprite2D.Play("shoot");
			_isShooting = true;
			
			_timerBetweenShots.WaitTime = WaitTimeBetweenShots;

			_timerBetweenShots.Start();
			// Add Blowback Impulse to player
			var player = GetParent<CharacterBody2D>();
			var normalizedDirection = new Vector2(-Mathf.Cos(Rotation), -Mathf.Sin(Rotation)).Normalized();
			GD.Print(normalizedDirection.Length());
			player.Velocity += normalizedDirection * Recoil;
 
		}

		return;

		void SpawnBullet()
		{
			Node2D bullet = BulletScene.Instantiate<Node2D>();
			AddChild(bullet);
			Vector2 offset;
			offset.X = Mathf.Cos(Rotation);
			offset.Y = Mathf.Sin(Rotation);
			bullet.GlobalPosition = GlobalPosition + offset * 100;
		}

		void AddScreenShake()
		{
			// This will get the parent and find a child node called "JUICE_ScreenShake"
			var screenShakeNode = GetParent().GetNode("ScreenShakeManager");

			// This will cast it to your JUICE_ScreenShake script type
			var screenShake = screenShakeNode as JUICE_ScreenShake;

			if (screenShake != null) // Just to make sure we got it
			{
				screenShake.StartShake(7.5f, 0.3f);
			}
		}
	}
	
}