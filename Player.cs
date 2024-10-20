using Godot;
using Microsoft.VisualBasic;
using System;

// Main things will be triggering a reel in mini game on a fishing pole that has a fish hooked
// putting the fish into storage on the boat thats it for now
// so they need to be able to move and do those things
public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 200; // How fast the player will move (pixels/sec).
	private Vector2 _screenSize; // Size of the game window.



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_screenSize = GetViewport().GetVisibleRect().Size;
		var velocity = new Vector2(); // The player's movement vector.
		// Move the player based on input.
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}
		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		var playerAnimation = GetNode<AnimatedSprite2D>("PlayerAnimatedSprite2D");

		// side to side and up and down walking animation
		if (velocity.Length() != 0)
		{
			playerAnimation.Play();
			if (velocity.X != 0)
			{
				playerAnimation.Animation = "walk";
				playerAnimation.FlipH = velocity.X < 0;
			}
			else if (velocity.Y != 0)
			{
				playerAnimation.Animation = "up";
				if (velocity.Y > 0)
				{
					playerAnimation.Animation = "up";
				}
			}
		}
		else
		{
			playerAnimation.Stop();
		}


		velocity = velocity.Normalized() * Speed;
		Position += velocity * (float)delta;
		Position = new Vector2(
			Mathf.Clamp(Position.X, 0, _screenSize.X),
			Mathf.Clamp(Position.Y, 0, _screenSize.Y)
		);
	}
}
