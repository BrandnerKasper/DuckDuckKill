using Godot;
using System;

public partial class ShotGun : Node2D
{
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
            
            GD.Print(Rotation);
            
            
            // get mouse position
            Vector2 mousePos = GetGlobalMousePosition();
            Vector2 playerPos = GlobalPosition;
            
         

            // calculate the direction
            float deltaX = mousePos.X - playerPos.X;
            float deltaY = mousePos.Y - playerPos.Y;

            // calculate the angle
            float angle = Mathf.Atan2(deltaY, deltaX);

            // // create a bullet
            // Bullet bullet = (Bullet)BulletScene.Instance();
            // bullet.GlobalPosition = playerPos;
            // bullet.Rotation = angle;
            // GetParent().AddChild(bullet);
        }
    }
    
    
}
