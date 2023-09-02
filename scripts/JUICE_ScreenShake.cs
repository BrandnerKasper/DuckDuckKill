using Godot;
using System;

public partial class JUICE_ScreenShake : Node
{
    private Camera2D _camera;  // The camera you wanna shake
    private double _shakeAmount = 0.0f; // How much to shake
    private double _shakeDuration = 0.0f; // How long to shake for
    private Vector2 _originalPosition; // Store the original camera position
    private Vector2 _currentOffset;
    private bool _isShaking = false;

    public override void _Ready()
    {
        _camera = GetParent().GetNode<Camera2D>("Camera2D");
        _originalPosition = _camera.Position;
    }
    
    public override void _Process(double delta)
    {
        
        // Update the shake effect
        if (_shakeDuration > 0)
        {
            if(!_isShaking)
            {
                _isShaking = true;
                _currentOffset = new Vector2((float)_shakeAmount, (float)_shakeAmount);
                _originalPosition = _camera.Position;
            }
            
            GD.Print("shake");
            // Randomize the camera's position a bit
            Vector2 shake = new Vector2((float)GD.RandRange(-_shakeAmount, _shakeAmount), (float)GD.RandRange(-_shakeAmount, _shakeAmount));
            _camera.Position = _originalPosition + shake;

            // Decrease the shake duration and amount
            _shakeDuration -= (float)delta;
            _shakeAmount = (float)Mathf.Lerp(_shakeAmount, 0, 1 - _shakeDuration / 0.5f); // Notice the parameter change here

        }
        else
        {
            _isShaking = false;
            _currentOffset = new Vector2(0, 0);
            _shakeAmount = 0.0f;
            _shakeDuration = 0.0f;
            _camera.Position = _originalPosition;
            
            _camera.PositionSmoothingEnabled = true;
        }
    }

    public  void StartShake(float amount, float duration)
    {
        _shakeAmount = amount;
        _shakeDuration = duration;
        _currentOffset = new Vector2(amount, amount );
        
        _camera.PositionSmoothingEnabled = false;
        //camera.Position = desiredPos;
    }
}
