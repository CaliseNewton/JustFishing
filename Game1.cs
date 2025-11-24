using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using JustFishing.GameObjects;

namespace JustFishing;

public class Game1 : Core
{
    // texture region that defines the slime sprite in the atlas.
    private Player _player;

    private SpriteFont _font;

    private Vector2 _playerPosition = new Vector2(0.5f * 1280, 0.5f * 720);

    public Game1() : base("Just Fishing", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("fonts/ScoreFont");

        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // initialize animation for player.
        AnimatedSprite playerSprite = atlas.CreateAnimatedSprite("idle");

        playerSprite.AddAnimation("idle", atlas.GetAnimation("idle"));
        playerSprite.AddAnimation("cast", atlas.GetAnimation("cast"));
        playerSprite.AddAnimation("waitingForBite", atlas.GetAnimation("waitingForBite"));
        playerSprite.AddAnimation("reeling", atlas.GetAnimation("reeling"));
        playerSprite.AddAnimation("caught", atlas.GetAnimation("caught"));
        playerSprite.Scale = new Vector2(4.0f, 4.0f);

        // create player
        _player = new Player(playerSprite);

        _player.Position = _playerPosition;
    }

    protected override void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();

        // Allows the game to exit
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (mouse.LeftButton == ButtonState.Pressed)
        {
            _player.StartCast();
        }

        _player.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        GraphicsDevice.Clear(Color.DarkOliveGreen);

        // Begin the sprite batch to prepare for rendering.
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the player texture region at a scale of 4.0
        _player.Draw();

        SpriteBatch.DrawString(_font, $"Score: {_player.FishCaught}", new Vector2(10, 10), Color.White);

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
