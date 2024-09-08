using System;

public static class SpinService
{
    public static void SetDate()
        => Progress.Inventory.spins.today = DateTime.Now.Date;
    public static bool CompareDate()
    {
        if (Progress.Inventory.spins == null)
            Progress.Inventory.spins = new();
        return DateTime.Now.Date == Progress.Inventory.spins.today;
    }
    public static void RemoveSpin()
        => Progress.Inventory.spins.spins -= 1;
    public static void RemoveRewardSpin()
        => Progress.Inventory.spins.rewardSpins = Math.Clamp(Progress.Inventory.spins.rewardSpins - 1, 0, 3);
    public static void ReloadSpinsOnNewDay()
        => Progress.Inventory.spins.NewDay();
}

