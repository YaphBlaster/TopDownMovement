using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    const int MAX_SPEED = 500;
    const int ACCELERATION = 2000;
    Vector2 motion = Vector2.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public Vector2 GetInputAxis()
    {
        // Initialize axis to a zero vector
        var axis = Vector2.Zero;

        // Clever way to get x axis movement
        // EX. holding stick to the right
        //      axis.x = 1 - 0 = 1
        // EX. holding stick to the left
        //      axis.x = 0 - 1 = -1
        axis.x = Convert.ToInt32(Input.IsActionPressed("ui_right")) - Convert.ToInt32(Input.IsActionPressed("ui_left"));

        // Clever way to get y axis movement
        // EX. holding stick down
        //      axis.y = 1 - 0 = 1
        // EX. holding stick up
        //      axis.y = 0 - 1 = -1

        axis.y = Convert.ToInt32(Input.IsActionPressed("ui_down")) - Convert.ToInt32(Input.IsActionPressed("ui_up"));

        // Normalize vector
        // Converts vector to make the vector's length 1
        // In this case, if this was not applied, diagonal direction would be result in faster movement than "cardinal" (pure x, pure y) directions
        return axis.Normalized();
    }

    public void ApplyFriction(float amount)
    {
        if (motion.Length() > amount)
        {
            // Normalize current motion and multiply by the friction amount
            // Go in the opposite direction to slow down the player
            // EX. amount = 10
            // motion -= (1 * 10)
            // NOTE: Normalization makes a vector's length 1
            motion -= motion.Normalized() * amount;
        }
        else
        {
            motion = Vector2.Zero;
        }
    }

    public void ApplyMovement(Vector2 acceleration)
    {
        // Apply acceleration to the Player
        motion += acceleration;

        // Clamp Player speed to a maximum of MAX_SPEED
        motion = motion.Clamped(MAX_SPEED);
    }

    public override void _PhysicsProcess(float delta)
    {
        var axis = GetInputAxis();

        // Analog stick at rest
        if (axis == Vector2.Zero)
        {
            // Apply friction to the character
            ApplyFriction(ACCELERATION * delta);
        }
        // Analog stick is moved
        else
        {
            // Apply movement derived from the axis, acceleration and delta (time between frames, always a very small number)
            ApplyMovement(axis * ACCELERATION * delta);
        }

        motion = MoveAndSlide(motion);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}




