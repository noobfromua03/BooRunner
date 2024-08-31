using System;

public static class WithExtensions
{
    //<summary>
    //Apply action for the T object
    //</summary>
    public static T With<T>(this T self, Action<T> set)
    {
        set.Invoke(self);
        return self;
    }

    //<summary>
    //Apply action for the T object if the condition is true
    //</summary>
    public static T With<T>(this T self, Action<T> apply, Func<bool> when)
    {
        if (when())
            apply?.Invoke(self);

        return self;
    }

    //<summary>
    //Apply action for the T object if the condition is true
    //</summary>
    public static T With<T>(this T self, Action<T> apply, bool when)
    {
        if (when)
            apply?.Invoke(self);

        return self;
    }
}