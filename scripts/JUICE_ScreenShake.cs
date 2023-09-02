using Godot;
using System;

public partial class JUICE_ScreenShake : Node
{
    private Camera2D _camera;  // The camera you wanna shake
    private float _shakeAmount = 0.0f; // How much to shake
    private float _shakeDuration = 0.0f; // How long to shake for
    private float _initialShakeDuration = 0.0f; // How long to shake for
    private Vector2 _originalOffset; // Store the original camera offset
    private bool _isShaking = false;

    public override void _Ready()
    {
        _camera = GetParent().GetNode<Camera2D>("Camera2D");
    }

    public override void _Process(double delta)
    {
        if (_shakeDuration != 0)
        {
            // set initial max offset
            if (!_isShaking)
            {
                _initialShakeDuration = _shakeDuration;
                // Randomize the offset within the shake amount
                float xOffset = (float)GD.RandRange(-_shakeAmount, _shakeAmount);
                float yOffset = (float)GD.RandRange(-_shakeAmount, _shakeAmount);
                _camera.Offset = _originalOffset + new Vector2(xOffset, yOffset);
            }

            // Decrease max Offset by tweening down to 0
            _shakeDuration -= (float)delta;
            _shakeAmount = Mathf.Lerp(_shakeAmount, 0, _initialShakeDuration-_shakeDuration);
            if(_isShaking)
                _isShaking = false;
            else
                _camera.Offset = _originalOffset + new Vector2(_shakeAmount, _shakeAmount);
            
            float xOffset2 = (float)GD.RandRange(-_shakeAmount, _shakeAmount);
            float yOffset2 = (float)GD.RandRange(-_shakeAmount, _shakeAmount);
            _camera.Offset = _originalOffset + new Vector2(xOffset2, yOffset2);
            
            if (_shakeDuration <= 0)
            {
                _isShaking = false;
                _shakeDuration = 0;
                _camera.Offset = _originalOffset; // Reset to original position
            }
        }
    }

    public void StartShake(float amount, float duration)
    {
        _shakeAmount = amount;
        _shakeDuration = duration;
    }
}