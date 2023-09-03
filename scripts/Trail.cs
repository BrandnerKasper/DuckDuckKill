using Godot;
using System;

public partial class Trail : Line2D
{
	const int MAX_POINTS = 2000;
	private Curve2D curve = new Curve2D();
  
	public override void _Ready()
	{
		// Initialize stuff here if needed
	}

	public override void _Process(double delta)
	{
		curve.AddPoint(((Node2D)GetParent()).Position);
		var bakedPoints = curve.GetBakedPoints();
		if (bakedPoints.Length > MAX_POINTS)
		{
			curve.RemovePoint(0);
		}
		Points = curve.GetBakedPoints();
	}

	public async void Stop()
	{
		SetProcess(false);
		var tw = GetTree().CreateTween();
		tw.TweenProperty(this, "modulate:a", 0.0f, 3.0f);
		await ToSignal(tw, "tween_completed");
		QueueFree();
	}

	public static Trail Create()
	{
		var scn = (PackedScene)GD.Load("res://trail.tscn");
		return (Trail)scn.Instantiate();
	}
}
