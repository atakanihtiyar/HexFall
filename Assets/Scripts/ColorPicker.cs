using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gokyolcu.Utilities;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private Color[] colorList = new Color[] {
        ConvertColorFrom255To1(112, 214, 255),
        ConvertColorFrom255To1(255, 112, 166),
        ConvertColorFrom255To1(255, 151, 112),
        ConvertColorFrom255To1(255, 248, 112),
        ConvertColorFrom255To1(171, 255, 143),
    };

    public Color GetRandomColor()
    {
        int randomColorIndex = Random.Range(0, colorList.Length);
        return GetColor(randomColorIndex);
    }

    public Color GetColor(int index)
    {
        try
        {
            return colorList[index];
        }
        catch (System.Exception e)
        {
            Debug.Log($"Error when color picking: {e}");
            return Color.white;
        }
    }
}
