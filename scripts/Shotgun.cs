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

    }
}
