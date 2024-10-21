using Godot;

public partial class Player : Area2D
{
    [Export]
    public int Speed { get; set; } = 200;

    private Vector2 _screenSize;

    public override void _Ready()
    {
        _screenSize = GetViewport().GetVisibleRect().Size;
    }

    public override void _Process(double delta)
    {
        var velocity = new Vector2();

        if (Input.IsActionPressed("move_right")) velocity.X += 1;
        if (Input.IsActionPressed("move_left")) velocity.X -= 1;
        if (Input.IsActionPressed("move_down")) velocity.Y += 1;
        if (Input.IsActionPressed("move_up")) velocity.Y -= 1;

        UpdateAnimation(velocity);

        velocity = velocity.Normalized() * Speed;
        Position += velocity * (float)delta;
        Position = new Vector2(
            Mathf.Clamp(Position.X, 0, _screenSize.X),
            Mathf.Clamp(Position.Y, 0, _screenSize.Y)
        );
    }

    private void UpdateAnimation(Vector2 velocity)
    {
        var playerAnimation = GetNode<AnimatedSprite2D>("PlayerAnimatedSprite2D");
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
            }
        }
        else
        {
            playerAnimation.Stop();
        }
    }
}
