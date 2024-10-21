using Godot;

public partial class FishingPole : Area2D
{
    public bool FishOnLine { get; private set; } = false;
    public bool FishCaught { get; private set; } = false;
    private ColorRect FishLine;

    public override void _Ready()
    {
        FishLine = GetNode<ColorRect>("LineColor");
        SetFishOnLine();
        Connect("area_entered", new Callable(this, nameof(OnFishingPoleAreaEntered)));
    }

    private void SetFishOnLine()
    {
        Timer timer = new Timer
        {
            WaitTime = 1,
            OneShot = false
        };
        timer.Connect("timeout", new Callable(this, nameof(OnFishCheckTimeout)));
        AddChild(timer);
        timer.Start();
    }

    private void OnFishCheckTimeout()
    {
        if (!FishOnLine && GD.Randi() % 100 < 20)
        {
            FishLine.Color = new Color(1, 0, 0); // Red indicates fish on line
            FishOnLine = true;
            GD.Print("Fish is on the line!");
        }
    }

    public void TryCatchFish()
{
    if (FishOnLine)
    {
        // Load the fishing mini-game scene
        var fishingMiniGame = (FishingMinigame)GD.Load<PackedScene>("res://fishing_minigame.tscn").Instantiate();
        GetTree().CurrentScene.AddChild(fishingMiniGame);
        fishingMiniGame.StartGame();

        // Reset the fishing line
        FishLine.Color = new Color(0, 1, 0); // Green indicates fish caught
        GD.Print("You caught a fish!");

        FishOnLine = false;
    }
    else
    {
        GD.Print("No fish to catch!");
    }
}

    private void OnResetFishLine()
    {
        FishLine.Color = new Color(1, 1, 1); // White indicates no fish on line
    }

    private void OnFishingPoleAreaEntered(Area2D area)
    {
        if (area is Player)
        {
            TryCatchFish();
        }
    }
}
