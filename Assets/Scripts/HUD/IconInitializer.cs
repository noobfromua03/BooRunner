using UnityEngine;
using UnityEngine.UI;

public class IconInitializer : MonoBehaviour
{
    [SerializeField] private IconType type;
    [SerializeField] private Image image;
    [SerializeField] private Animator animator;

    private void Start()
        => image.sprite = IconsConfig.Instance.GetByType(type);

    public void Initialize(IconType type)
    {
        this.type = type;
        image.sprite = IconsConfig.Instance.GetByType(this.type);
    }

    public void Fading()
        => animator.SetBool("Fading", true);

    public void FadingOff()
        => animator.SetBool("Fading", false);

    public IconType GetIconType()
        => type;

}