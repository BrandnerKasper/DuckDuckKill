using Godot;

namespace DuckDuckKill.scripts;

internal enum States
{
    Idle,
    Move,
    StartJump,
    Jump,
    StartCharge,
    HoldingCharge,
    ReleaseCharge
}

public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 200.0f;
    [Export] public float Acceleration = 15.0f;
    [Export] public float Deceleration = 25.0f;
    [Export] public float JumpVelocity = -300.0f;
    [Export] public float PushForce = 200.0f;
    
    // Signals
    // [Signal]
    // public delegate void OnAnimationFinishedEventHandler(string animName);

    private AnimatedSprite2D _aSprite;
    private Vector2 _velocity;
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private States _currentState = States.Idle;
    private States _oldStateDEBUG;

    private string _animName = "";
    
    // Audio
    private AudioStreamPlayer _jumpQuack;
    // private AudioStreamPlayer _
    
    public override void _Ready()
    {
        // Animations
        _aSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _aSprite.AnimationLooped += ASpriteOnAnimationFinished;
        _aSprite.Animation = "idle";
        _aSprite.Play();
        
        // Audio
        _jumpQuack = new AudioStreamPlayer();
        _jumpQuack.Stream = GD.Load<AudioStream>("res://assets/audio/quack.wav");
        _jumpQuack.VolumeDb = Mathf.LinearToDb(0.8f);
        AddChild(_jumpQuack);
    }



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("blast"))
        {
            // TODO: attack triggered -> can move again -> spawn area2d to apply force
            _currentState = States.ReleaseCharge;
        }

        if (@event.IsActionPressed("blast"))
        {
            _currentState = States.StartCharge;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            _currentState = States.StartJump;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_currentState != _oldStateDEBUG)
        {
            GD.Print($"State: {_currentState}");
            _oldStateDEBUG = _currentState;
        }

        // Add the gravity.
        if (!IsOnFloor())
            _velocity.Y += _gravity * (float)delta;
        Move();
        Jump();
        
        Velocity = _velocity;
        _velocity = Velocity;

        HandleAnimation();

        if (MoveAndSlide())
            HandleCollision();
    }

    private void Move()
    {
        if (_currentState != States.Idle && _currentState != States.Move)
        {
            return;
        }
        // Get the input direction and handle the movement/deceleration.
        Vector2 direction = Input.GetVector("left", "right", "down", "up").Normalized();
        if (direction != Vector2.Zero)
        {
            _velocity.X = Mathf.MoveToward(Velocity.X, (direction.X * Speed), Acceleration);
        }
        else
        {
            _velocity.X = Mathf.MoveToward(Velocity.X, 0, Deceleration);
        }
        
        if (Velocity.X == 0.0f)
        {
            _currentState = States.Idle;
        }
        else
        {
            _currentState = States.Move;
        }
    }

    private void Jump()
    {
        if (_currentState != States.Jump)
        {
            return;
        }
        _velocity.Y = JumpVelocity;
        _currentState = States.Idle;
        _jumpQuack.Play();
    }

    private void HandleAnimation()
    {
        if (Velocity.X == 0.0f)
        {
		}
        else
        {
            _aSprite.FlipH = Velocity.X < 0;
        }

        switch (_currentState)
        {
            case States.Idle:
                _aSprite.Play("idle");
                _animName = "idle";
                break;
            case States.Move:
                // _aSprite.FlipH = Velocity.X < 0;
                _aSprite.Play("walk");
                _animName = "walk";
                break;
            case States.StartJump:
                _aSprite.Play("jump");
                _animName = "jump";
                break;
            case States.StartCharge:
                _aSprite.Play("startCharge");
                _animName = "startCharge";
                break;
            case States.HoldingCharge:
                _aSprite.Play("holdCharge");
                _animName = "holdCharge";
                break;
            case States.ReleaseCharge:
                _aSprite.Play("releaseCharge");
                _animName = "releaseCharge";
                break;
            default:
                _currentState = States.Idle;
                break;
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

                RigidBody2D collisionObject = collision.GetCollider() as RigidBody2D;
                collisionObject?.ApplyImpulse(collision.GetNormal() * -PushForce);
            }
        }
    }
    
    
    private void ASpriteOnAnimationFinished()
    {
        // GD.Print(_animName);
        if (_animName.Equals("startCharge"))
        {
            _currentState = States.HoldingCharge;
            _aSprite.Pause();
        }

        if (_animName.Equals("releaseCharge"))
        {
            _currentState = States.Idle;
            _aSprite.Pause();
        }

        if (_animName.Equals("jump"))
        {
            _currentState = States.Jump;
            _aSprite.Pause();
        }
    }
    
}