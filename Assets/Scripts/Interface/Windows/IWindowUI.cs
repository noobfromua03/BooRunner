using UnityEngine;

public interface IWindowUI
{
    public WindowType Type { get; }

    public GameObject Window { get; }

    public void DestroySelf();
}
