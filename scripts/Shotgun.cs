using Godot;
using System;

public partial class ShotGun : Node2D
{
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
        if (@event.IsActionPressed("mouse_left"))
        {
            SpawnBullet();
            AddScreenShake();
            // Add Blowback Impulse to player
            var player = GetParent<CharacterBody2D>();
            player.Velocity += new Vector2(-Mathf.Cos(Rotation), -Mathf.Sin(Rotation)) * 100;
            
            
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
