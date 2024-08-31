using UnityEngine;
using UnityEngine.UI;

public class IconInitializer : MonoBehaviour
{
    [SerializeField] private IconType type;
    [SerializeField] private Image image;

    private void Start()
        => image.sprite = IconsConfig.Instance.GetByType(type);

    public void Initialize(IconType type)
    {
        this.type = type;
        image.sprite = IconsConfig.Instance.GetByType(this.type);
    }
    public IconType GetIconType()
        => type;

}