
using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using MonoGame.Framework.Devices.Sensors;
using Microsoft.Xna.Framework.Graphics;

namespace JustFishing.GameObjects;

public class Player
{
    public PlayerState State = PlayerState.Idle;

    private float castTimer = 0f;
    private readonly float castDuration = 0.8f;

    /// <summary>
    /// Gets or Sets the position of the player.
    /// </summary>
    public Vector2 Position { get; set; }


    private AnimatedSprite _sprite;

    public Player(AnimatedSprite sprite)
    {
        // Initialize player with sprite
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
            case PlayerState.Casting:
                castTimer += elapsed;

                if (castTimer >= castDuration)
                    FinishCast();
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
}
