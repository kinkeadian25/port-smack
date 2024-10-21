using Godot;
using System;

public partial class FishingMinigame : Node2D
{
	[Export]
	public float GameDuration { get; set; } = 5f; // Duration of the game in seconds
	[Export]
	public float SlideSpeed { get; set; } = 200f; // Speed of the sliding rectangles
	[Export]
	public int TargetHits { get; set; } = 5; // Number of hits required

	private bool _gameActive;
	private Timer _timer;
	private Label _timerLabel;
	private Label _hitCountLabel;
	private Label _targetHitsLabel;
	private ColorRect _fishArea;
	private ColorRect _reelArea;
	private ColorRect _targetArea;
	private ColorRect _movementArea;
	private int _currentHits;
	private bool _isSliding;
	private Vector2 _fishDirection = Vector2.Up; // Changed to vertical
	private Vector2 _reelDirection = Vector2.Down; // Changed to vertical

	public override void _Ready()
	{
		_currentHits = 0;
		_gameActive = false;

		_timer = GetNode<Timer>("Timer");
		_timer.Connect("timeout", new Callable(this, nameof(OnGameTimeout)));

		_timerLabel = GetNode<Label>("TimerLabel");
		_hitCountLabel = GetNode<Label>("HitCountLabel");
		_targetHitsLabel = GetNode<Label>("TargetHitsLabel");
		_fishArea = GetNode<ColorRect>("FishArea");
		_reelArea = GetNode<ColorRect>("ReelArea");
		_targetArea = GetNode<ColorRect>("TargetArea");
		_movementArea = GetNode<ColorRect>("MovementArea");
	}

	public void StartGame()
	{
		_gameActive = true;
		_currentHits = 0;
		_timer.Start(GameDuration);
		UpdateLabels();
		GD.Print("Mini-game started! Hit space bar when they overlap!");

		// Start sliding the rectangles
		_isSliding = true;
	}

	public override void _Process(double delta)
	{
		if (_gameActive)
		{
			SlideRectangles(delta);

			if (Input.IsActionJustPressed("ui_accept")) // Space key by default
			{
				CheckHit();
			}
		}
	}

	private void SlideRectangles(double delta)
{
    // Move the fish area
    _fishArea.Position += _fishDirection * SlideSpeed * (float)delta;
    _reelArea.Position += _reelDirection * SlideSpeed * (float)delta;

    // Check the bounds of the movement area for the fish area
    if (_fishArea.Position.Y + _fishArea.Size.Y > _movementArea.Position.Y + _movementArea.Size.Y)
        _fishDirection = new Vector2(0, -1); // Change direction to up
    else if (_fishArea.Position.Y < _movementArea.Position.Y)
        _fishDirection = new Vector2(0, 1); // Change direction to down

    // Check the bounds of the movement area for the reel area
    if (_reelArea.Position.Y + _reelArea.Size.Y > _movementArea.Position.Y + _movementArea.Size.Y)
        _reelDirection = new Vector2(0, -1); // Change direction to up
    else if (_reelArea.Position.Y < _movementArea.Position.Y)
        _reelDirection = new Vector2(0, 1); // Change direction to down
}

	private void CheckHit()
	{
		// Check if the two rectangles overlap within the target area
		if (_fishArea.GetRect().Intersects(_reelArea.GetRect()) && _fishArea.GetRect().Intersects(_targetArea.GetRect()) && _reelArea.GetRect().Intersects(_targetArea.GetRect()))
		{
			_currentHits++;
			GD.Print("Hit registered! Current hits: " + _currentHits);
			UpdateLabels();

			if (_currentHits >= TargetHits)
			{
				GD.Print("You caught the fish!");
				EndGame();
			}
		}
		else
		{
			GD.Print("Missed! Try again.");
		}
	}

	private void OnGameTimeout()
	{
		GD.Print("Time's up! You didn't catch the fish.");
		EndGame();
	}

	private void UpdateLabels()
	{
		_timerLabel.Text = "Time: " + (int)(_timer.TimeLeft) + "s";
		_hitCountLabel.Text = "Hits: " + _currentHits + " / Target: " + TargetHits;
	}

	private void EndGame()
	{
		_gameActive = false;
		_isSliding = false; // Stop sliding
		// Logic to handle what happens after the mini-game ends
		GetTree().ChangeSceneToFile("res://MainScene.tscn"); // Go back to main scene or whatever is appropriate
	}
}
