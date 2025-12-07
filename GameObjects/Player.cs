
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using System.Diagnostics;

namespace JustFishing.GameObjects;

public class Player
{
    public PlayerState State = PlayerState.Idle;

    private float castTimer = 0f;
    private readonly float castDuration = 0.8f;

    private float waitTimer = 0f;
    private readonly float waitDuration = 0.8f;

    private Random _randomWaitMultiplier;


    /// <summary>
    /// Gets or Sets the position of the player.
    /// </summary>
    public Vector2 Position { get; set; }

    private AnimatedSprite _sprite;

    // Movement parameters
    private float _speed = 240f;   // pixels per second
    private float _accel = 10f;    // smoothing movement
    private float _decel = 10f;

    private Vector2 _velocity;
    private Vector2 _direction;

    // Input buffer
    private Queue<Vector2> _inputBuffer;
    private const int MAX_BUFFER = 4;
    private const float BUFFER_TIME = 0.12f; // 120ms

    private float _bufferTimer;
    private bool _isMovementKeyDown = false;


    public Player(AnimatedSprite sprite, Vector2 startingPosition)
    {
        // Initialize player with sprite
        _randomWaitMultiplier = new Random();
        _sprite = sprite;
        Position = startingPosition;
        _inputBuffer = new Queue<Vector2>();
        _sprite.Play("idle");
    }

    private void HandleInput()
    {
        Vector2 dir = Vector2.Zero;
        _isMovementKeyDown = false;

        if (GameController.MoveUp())
        {
            dir.Y -= 1;
            _isMovementKeyDown = true;
            State = PlayerState.WalkingUp;
        }

        if (GameController.MoveDown())
        {
            dir.Y += 1;
            _isMovementKeyDown = true;
            State = PlayerState.WalkingDown;
        }
        if (GameController.MoveLeft())
        {
            dir.X -= 1;
            _isMovementKeyDown = true;
            State = PlayerState.WalkingLeft;
        }
        if (GameController.MoveRight())
        {
            dir.X += 1;
            _isMovementKeyDown = true;
            State = PlayerState.WalkingRight;
        }

        if (GameController.Action())
        {
            if (State != PlayerState.Reeling)
            {
                StartCast();
            }
        }

        if (dir != Vector2.Zero)
        {
            dir.Normalize();

            if (_inputBuffer.Count < MAX_BUFFER)
            {
                _inputBuffer.Enqueue(dir);
            }

            _bufferTimer = BUFFER_TIME;
        }
    }

    private void ProcessBufferedInput(float dt)
    {
        if (_inputBuffer.Count > 0)
        {
            // consume buffered input when allowed
            _direction = _inputBuffer.Dequeue();
        }
        else if (!_isMovementKeyDown && State.IsMoving())
        {
            // no key pressed and no buffered directions → STOP
            _direction = Vector2.Zero;
            State = PlayerState.Idle;
        }

        // buffer expires after small time
        _bufferTimer -= dt;
        if (_bufferTimer <= 0)
        {
            _inputBuffer.Clear();
        }
    }

    private void UpdateVelocity(float dt)
    {
        if (_direction != Vector2.Zero)
        {
            // accelerate towards direction
            _velocity = Vector2.Lerp(_velocity, _direction * _speed, _accel * dt);
        }
        else
        {
            // decelerate
            _velocity = Vector2.Lerp(_velocity, Vector2.Zero, _decel * dt);
        }
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

        HandleInput();
        ProcessBufferedInput(elapsed);
        UpdateVelocity(elapsed);

        Position += _velocity * elapsed;

        switch (State)
        {
            case PlayerState.Idle:
                _sprite.Play("idle");
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
                }
                break;
            case PlayerState.Reeling:
                // Reeling logic can be added here
                break;
            case PlayerState.WalkingUp:
                _sprite.Play("walkUpDown");
                break;
            case PlayerState.WalkingDown:
                _sprite.Play("walkUpDown");
                break;
            case PlayerState.WalkingLeft:
                _sprite.Play("walkRight");
                _sprite.Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                break;
            case PlayerState.WalkingRight:
                _sprite.Play("walkRight");
                _sprite.Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
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

    public void CatchFish()
    {
        State = PlayerState.CaughtFish;
        _sprite.Play("caught");
    }

    public void FishEscape()
    {
        _sprite.Play("idle");
        State = PlayerState.Idle;
    }
}
