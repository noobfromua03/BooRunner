using System.Collections.Generic;
using UnityEngine;

public class ColorConfig : AbstractConfig<ColorConfig>
{
    [field: SerializeField] public List<Color32> Colors { get; private set; }
}
