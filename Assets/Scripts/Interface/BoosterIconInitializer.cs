using UnityEngine;
public class BoosterIconInitializer : MonoBehaviour
{
    [SerializeField] public IconInitializer iconInitializer;
    [SerializeField] private Animator animator;

    public Coroutine Coroutine;

    public void Fading()
        => animator.SetBool("Fading", true);

    public void FadingOff()
        => animator.SetBool("Fading", false);

    public IconInitializer GetInitializer()
        => iconInitializer;
}

