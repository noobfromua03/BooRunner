using System;

public static class CurrencyService
{
    public static Action<int> OnSoftChanged;
    public static Action<int> OnHardChanged;

    public static int GetCurrency(CurrencyType type)
    {
        return type switch
        {
            CurrencyType.Soft => Progress.Inventory.soft,
            CurrencyType.Hard => Progress.Inventory.hard,
            _ => 0
        };
    }

    public static void AddCurrency(CurrencyType type, int amount)
    {
        switch (type)
        {
            case CurrencyType.Soft:
                Progress.Inventory.soft += amount;
                OnSoftChanged?.Invoke(Progress.Inventory.soft);
                break;
            case CurrencyType.Hard:
                Progress.Inventory.hard += amount;
                OnHardChanged?.Invoke(Progress.Inventory.hard);
                break;
        }
    }

    public static void RemoveCurrency(CurrencyType type, int amount)
    {
        switch (type)
        {
            case CurrencyType.Soft:
                Progress.Inventory.soft -= amount;
                OnSoftChanged?.Invoke(Progress.Inventory.soft);
                break;
            case CurrencyType.Hard:
                Progress.Inventory.hard -= amount;
                OnHardChanged?.Invoke(Progress.Inventory.hard);
                break;
        }
    }
}