using Godot;
using System;

public partial class Bullet : Node2D
{
    private float _lifeTime = 5;
    public override void _Process(double delta)
    {
        // move in the direction of the _rotation
        Vector2 velocity = new Vector2(0, 0);
        velocity.X = Mathf.Cos(Rotation);
        velocity.Y = Mathf.Sin(Rotation);
        velocity = velocity.Normalized() * 10;
        Position += velocity * (float)delta;
        
        // destroy after _lifeTime
        _lifeTime -= (float)delta;
        if (_lifeTime <= 0) QueueFree();
    }
}
