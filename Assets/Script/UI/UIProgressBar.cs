using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{

    [SerializeField] private Image progress_image;

    public void SetProgressValues(float value)
    {
        progress_image.fillAmount = value;
    }

}
