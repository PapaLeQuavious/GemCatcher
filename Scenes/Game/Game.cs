using Godot;
using System;

public partial class Game : Node2D
{

	const double GEM_MARGIN = 50.0;

	// Preload sounds that change at runtime like this to ensure performace
	private static readonly AudioStream EXPLODE_SOUND = GD.Load<AudioStream>("res://assets/explode.wav");
	// [Export] private AudioStream _explodeSound;

	// [Export] private Gem _gem;
	[Export] private PackedScene _gemScene;
	[Export] private Timer _spawnTimer;
	[Export] private Label _scoreLabel;
	[Export] private AudioStreamPlayer _music;
	[Export] private AudioStreamPlayer2D _effects;
	[Signal] public delegate void OnGemSpawnEventHandler();

	private int _score = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Gem gem = GetNode<Gem>("Gem");
		// _gem.OnScored += OnGemScored;
		_spawnTimer.Timeout += SpawnGem;
		SpawnGem();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SpawnGem()
	{
		// Gem type called gem. Error unless you cast the instantiated packed scene as a Gem type
		Gem gem = (Gem)_gemScene.Instantiate();
		AddChild(gem);

		// Good practice to set position AFTER adding as a child to prevent offset bugs
		Rect2 vpr = GetViewportRect(); // Get Viewport
		float rX = (float)GD.RandRange(vpr.Position.X + GEM_MARGIN, vpr.End.X - GEM_MARGIN); // Cast random X within vpr to float
		gem.Position = new Vector2(rX, -100); // Set random X start position
		gem.OnScored += OnGemScored; // Connect each child gem to the OnScored signal bus
		gem.OnGemOffScreen += GameOver; // Connect each child gem to the OnGemOffScreen signal
	}

	private void OnGemScored()
	{
		GD.Print("OnScored Received!");
		_score += 1;
		_scoreLabel.Text = $"{_score:0000}";
		_effects.Play();
	}

	private void GameOver()
	{
		GD.Print("Game Over");
		_spawnTimer.Stop();
		_music.Stop();

		_effects.Stop();
		_effects.Stream = EXPLODE_SOUND;
		_effects.Play();

		foreach (Node node in GetChildren())
		{
			node.SetProcess(false);
		}
	}
}
