using Godot;
using System;

public partial class FishingPole : Area2D
{
	public bool FishOnLine { get; private set; } = false;
	public bool FishCaught { get; private set; } = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetFishOnLine();
		Connect("area_entered", new Callable(this, nameof(OnFishingPoleAreaEntered)));
	}

	private void SetFishOnLine()
	{
		Timer timer = new Timer();
		timer.WaitTime = 1; // Set time for checking fish
		timer.OneShot = false;
		timer.Connect("timeout", new Callable(this, nameof(OnFishCheckTimeout)));
		AddChild(timer);
		timer.Start();
	}

	private void OnFishCheckTimeout()
	{
		if (!FishOnLine)
		{
			if (GD.Randi() % 100 < 25) // 25% chance to have fish on the line
			{
				FishOnLine = true;
				GD.Print("Fish is on the line!"); // Indicate fish is on the line
			}
		}
	}

	public void TryCatchFish()
	{
		if (FishOnLine)
		{
			FishCaught = true;
			FishOnLine = false; // Remove fish from the line
			GD.Print("You caught a fish!"); // Indicate fish is caught
		}
		else
		{
			GD.Print("No fish to catch!"); // Indicate no fish is available
		}
	}

	// Detect when the player enters the fishing pole's area
	private void OnFishingPoleAreaEntered(Area2D area)
	{
		GD.Print("Player entered fishing pole area");
		if (area is Player)
		{
			TryCatchFish(); // Attempt to catch fish when player enters
		}
	}
}
