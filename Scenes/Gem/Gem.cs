using Godot;
using System;
using System.Diagnostics;

public partial class Gem : Area2D
{
	// Exporting Variable for the Godot Editor
	[Export] float _speed = 100.0f;
	// Creating custom signal for scoring
	[Signal] public delegate void OnScoredEventHandler();
	[Signal] public delegate void OnGemOffScreenEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connecting signals to methods
		AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(0, _speed * (float)delta);
		CheckHitBottom();
	}

	// Method to handle area entered signal
	private void OnAreaEntered(Area2D area)
	{
		GD.Print("Gem collected!");
		EmitSignal(SignalName.OnScored); // Emit OnScored signal
		QueueFree(); // Remove the gem from the scene once collected
	}

	private void CheckHitBottom()
	{
		Rect2 vpr = GetViewportRect();
		if (Position.Y > vpr.End.Y) {
			EmitSignal(SignalName.OnGemOffScreen); // Emit OnGemOffScreen signal
			SetProcess(false); // Set process to false to stop the game from processing this gem any further
			QueueFree(); // Remove the Gem from the scene once hit bottom.
		}
	}
}
