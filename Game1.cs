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
        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // initialize animation for player.
        AnimatedSprite playerSprite = atlas.CreateAnimatedSprite("idle");

        playerSprite.AddAnimation("idle", atlas.GetAnimation("idle"));
        playerSprite.AddAnimation("cast", atlas.GetAnimation("cast"));
        playerSprite.AddAnimation("waitingForBite", atlas.GetAnimation("waitingForBite"));
        playerSprite.Scale = new Vector2(4.0f, 4.0f);

        // create player
        _player = new Player(playerSprite);

    }

    protected override void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();

        // Allows the game to exit
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (mouse.LeftButton == ButtonState.Pressed && (_player.State == PlayerState.Idle || _player.State == PlayerState.WaitingForBite))
        {
            _player.StartCast();
        }

        _player.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the slime texture region at a scale of 4.0
        _player.Draw();

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
