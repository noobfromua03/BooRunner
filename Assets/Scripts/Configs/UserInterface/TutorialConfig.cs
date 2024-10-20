﻿using System.Collections.Generic;
using UnityEngine;

public class TutorialConfig : AbstractConfig<TutorialConfig>
{
    [field: SerializeField] public List<SlideData> Slides { get; private set; }
}

[System.Serializable]
public class SlideData
{
    [field: SerializeField] public Sprite Sprite { get; private set; }


    [field: SerializeField][field: TextArea(1, 5)] public string Text { get; private set; }
}