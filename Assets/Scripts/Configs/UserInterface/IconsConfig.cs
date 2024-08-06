using System.Collections.Generic;
using UnityEngine;

public class IconsConfig : AbstractConfig<IconsConfig>
{
    [field: SerializeField] public List<IconData> Icons { get; private set; }


    public Sprite GetByType(IconType type)
    {
        var icon = Icons.Find(i => i.Type == type);
        if(icon == null)
            Debug.LogError($"Current type: {type} dont exist in Icons");

        return icon.Sprite;
    }
}

