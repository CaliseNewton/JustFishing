
using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;

namespace JustFishing.GameObjects;

public class Player
{
    public PlayerState State = PlayerState.Idle;

    private float castTimer = 0f;
    private readonly float castDuration = 0.8f;

    private float waitTimer = 0f;
    private readonly float waitDuration = 0.8f;

    private Random _randomWaitMultiplier;

    private int fishCaught = 0;

    /// <summary>
    /// Gets or Sets the position of the player.
    /// </summary>
    public Vector2 Position { get; set; }


    private AnimatedSprite _sprite;

    public Player(AnimatedSprite sprite)
    {
        // Initialize player with sprite
        _randomWaitMultiplier = new Random();
        _sprite = sprite;
        _sprite.Play("idle");
    }

    public void StartCast()
    {
        State = PlayerState.Casting;
        castTimer = 0f;

        _sprite.Play("cast");
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _sprite.Update(gameTime);

        switch (State)
        {
            case PlayerState.Idle:
                // Idle logic can be added here
                break;
            case PlayerState.Casting:
                castTimer += elapsed;

                if (castTimer >= castDuration)
                    FinishCast();
                break;
            case PlayerState.WaitingForBite:
                // Waiting for bite logic can be added here
                waitTimer += elapsed;
                if (waitTimer >= waitDuration * _randomWaitMultiplier.Next(4, 10))
                {
                    // Simulate a fish bite after waiting
                    State = PlayerState.Reeling;
                    _sprite.Play("reeling");
                    waitTimer = 0f;
                    ReelInFish();
                }
                break;
            case PlayerState.Reeling:
                // Reeling logic can be added here
                break;
        }
    }

    public void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, Position);
    }

    private void FinishCast()
    {
        State = PlayerState.WaitingForBite;
        _sprite.Play("waitingForBite");
    }

    private void ReelInFish()
    {
        //TODO: Add success/failure logic for catching fish.
            //Will be a tension minigame, white = good, red = bad / break line and lose fish.
            //Also some visual for the fish getting closer to the surface.

        State = PlayerState.Reeling;
        _sprite.Play("caught");
        fishCaught++;
        System.Diagnostics.Debug.WriteLine($"Fish caught! Total: {fishCaught}");
    }
}
