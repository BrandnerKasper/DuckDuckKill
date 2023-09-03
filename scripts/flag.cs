using Godot;
using System;

public partial class flag : Area2D
{
	// Anim
    private AnimatedSprite2D _aSprite;
	// Audio
	private AudioStreamPlayer _victory;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		_aSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_aSprite.Play("default");
		
		_victory = new AudioStreamPlayer();
		_victory.Stream = GD.Load<AudioStream>("res://assets/audio/victory.wav");
		_victory.VolumeDb = Mathf.LinearToDb(0.9f);
		AddChild(_victory);
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print(body.Name);
		_victory.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
