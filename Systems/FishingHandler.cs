using System;
using Microsoft.Xna.Framework;

public class FishingHandler
{
    public bool IsFishing { get; private set; } = false;

    private float tension;
    private float reelPower;
    private float fishStrength;
    private readonly float escapeThreshold = 100f;
    private readonly float catchThreshold = 100f;

    public event Action OnCaught;
    public event Action OnEscaped;

    public void StartFishing(float reelPower, float fishStrength)
    {
        IsFishing = true;
        tension = 0f;
        this.fishStrength = fishStrength;
        this.reelPower = reelPower;
    }

    public void StopFishing()
    {
        IsFishing = false;
    }

    public void UpdateFishing(GameTime gameTime, bool isReeling)
    {
        if (!IsFishing) return;

        float _elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (isReeling)
        {
            tension += reelPower * _elapsed;
        }
        else
        {
            tension -= fishStrength * _elapsed;
        }
        // Check for escape or catch conditions
        if (tension <= -escapeThreshold)
        {
            // Fish escapes
            IsFishing = false;
            OnEscaped?.Invoke();
            System.Diagnostics.Debug.WriteLine("The fish escaped!");
        }
        else if (tension >= catchThreshold)
        {
            // Fish is caught
            IsFishing = false;
            OnCaught?.Invoke();
            System.Diagnostics.Debug.WriteLine("You caught the fish!");
        }
    }

}