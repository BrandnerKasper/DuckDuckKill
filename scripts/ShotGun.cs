using Godot;
using System;
using System.Linq;

public partial class ShotGun : Node2D
{
	private AnimatedSprite2D _animatedSprite2D;
	private Timer _timerBetweenShots;
	bool isShooting = false;
	[Export] public float WaitTimeBetweenShots = 0.5f;
	private PackedScene _bullet = (PackedScene)GD.Load("res://scenes//bulletManager.tscn");

	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_timerBetweenShots = GetNode<Timer>("TimerBetweenShots");
		_timerBetweenShots.Timeout += () => isShooting = false;
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
		if (!isShooting && @event.IsActionPressed("mouse_left"))
		{
			Shoot();
			AddScreenShake();
			_animatedSprite2D.Play("shoot");
			isShooting = true;
			_animatedSprite2D.AnimationLooped += 
				() => _animatedSprite2D.Play("default");
	
			_timerBetweenShots.WaitTime = WaitTimeBetweenShots;

			_timerBetweenShots.Start();
			// Add Blowback Impulse to player
			var player = GetParent<CharacterBody2D>();
			var normalizedDirection = new Vector2(-Mathf.Cos(Rotation), -Mathf.Sin(Rotation)).Normalized();
			GD.Print(normalizedDirection.Length());
			player.Velocity += normalizedDirection * 500;
 
		}

		return;
		
		void Shoot()
		{
			var bulletParent = (Node2D) _bullet.Instantiate();
			GetTree().Root.AddChild(bulletParent);
			float offset = 40.0f;
			Vector2 spawnPosition =
				GlobalPosition + new Vector2((float)Math.Cos(Rotation) * offset, (float)Math.Sin(Rotation) * offset);

			bulletParent.Position = spawnPosition;
			bulletParent.Rotation = Rotation;
			var x = bulletParent.GetChildren().OfType<Bullet>();
			foreach (var bullet in x)
			{
				bullet.Start(Rotation);
			}
			
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