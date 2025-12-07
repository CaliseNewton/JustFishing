public enum PlayerState
{
    Idle,
    Casting,
    WaitingForBite,
    Reeling,
    CaughtFish,
    WalkingUp,
    WalkingDown,
    WalkingLeft,
    WalkingRight
}

public static class PlayerStateExtensions
{
    public static bool IsMoving(this PlayerState state)
    {
        return state == PlayerState.WalkingUp
            || state == PlayerState.WalkingDown
            || state == PlayerState.WalkingLeft
            || state == PlayerState.WalkingRight;
    }

    public static bool IsFishing(this PlayerState state)
    {
        return state == PlayerState.Casting
            || state == PlayerState.WaitingForBite
            || state == PlayerState.Reeling
            || state == PlayerState.CaughtFish;
    }
}
