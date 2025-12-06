using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;

public class FishingHandler
{
    public bool IsFishing { get; private set; } = false;
    public float CurrentDepth { get; private set; } = 0f;
    private float reelPower;
    private float fishStrength;
    private readonly float escapeThreshold = -100f;
    private readonly float catchThreshold = 100f;
    public float Tension { get; private set; } = 0f;
    public int FishCaught { get; private set; } = 0;
    public event Action OnCaught;
    public event Action OnEscaped;
    private float hudYPosition = 50f;
    private AnimatedSprite _sprite;
    private AnimatedSprite _fishHud;

    public FishingHandler(AnimatedSprite sprite, AnimatedSprite fishHud)
    {
        // Initialize player with sprite
        _sprite = sprite;
        _fishHud = fishHud;
        _sprite.Play("fishFlop");
    }

    public void StartFishing(float reelPower, float fishStrength)
    {
        IsFishing = true;
        CurrentDepth = 0f;
        this.fishStrength = fishStrength;
        this.reelPower = reelPower;
        Tension = 0f;
    }

    public void StopFishing()
    {
        IsFishing = false;
    }

    public void UpdateFishing(GameTime gameTime, bool isReeling)
    {
        if (!IsFishing) return;

        _fishHud.Update(gameTime);
        _sprite.Update(gameTime);

        float _elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (isReeling)
        {
            CurrentDepth += reelPower * _elapsed;
            Tension += reelPower * 0.1f * _elapsed;
        }
        else
        {
            CurrentDepth -= fishStrength * _elapsed;
            if (Tension > 0f)
            {
                Tension -= fishStrength * 0.2f * _elapsed;
            }
            
        }
        // Check for escape or catch conditions
        if (CurrentDepth <= escapeThreshold)
        {
            // Fish escapes
            IsFishing = false;
            OnEscaped?.Invoke();
            System.Diagnostics.Debug.WriteLine("The fish escaped!");
        }

        else if (Tension > 10f)
        {
            // Fish escapes due to high tension
            IsFishing = false;
            OnEscaped?.Invoke();
            System.Diagnostics.Debug.WriteLine("The fish escaped because the line snapped!");
        }
        else if (CurrentDepth >= catchThreshold)
        {
            // Fish is caught
            IsFishing = false;
            OnCaught?.Invoke();
            FishCaught++;
            System.Diagnostics.Debug.WriteLine("You caught the fish!");
        }
    }

    public void Draw()
    {
        if (!IsFishing) return;

        HelpDrawHud();
    }

    private void HelpDrawHud()
    {
        float t = (CurrentDepth - escapeThreshold) / (catchThreshold - escapeThreshold);
        t = MathHelper.Clamp(t, 0f, 1f);

        float fishY = MathHelper.Lerp(hudYPosition + 64f, hudYPosition, t);


        float normaliseTension = (Tension - 0f) / (10f - 0f);
        normaliseTension = MathHelper.Clamp(normaliseTension, 0f, 1f);

        // float red = MathHelper.Lerp(255f, 0f, normaliseTension);

        // Console.WriteLine(red);

        Color tint = new Color(1f, 1f - normaliseTension, 1f - normaliseTension, 255f);
        _fishHud.Color = tint;

        _fishHud.Draw(Core.SpriteBatch, new Vector2(400, hudYPosition));
        _sprite.Draw(Core.SpriteBatch, new Vector2(400, fishY));
    }

}