using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public RectTransform ImgRedBack;
    public RectTransform ImgGreen;

    public void ReducingHealthStrip(float healthHull, float takeDamage)
    {
        float imgOffcet = takeDamage * 100 / healthHull;
        if(ImgGreen.localScale.x - imgOffcet / 100 > 0)
        {
            ImgGreen.localScale = new Vector3(ImgGreen.localScale.x - imgOffcet / 100, ImgGreen.localScale.y, ImgGreen.localScale.z);
            ImgGreen.transform.localPosition = new Vector3(ImgGreen.localPosition.x - imgOffcet / 2, ImgGreen.localPosition.y, ImgGreen.localPosition.z);
        }
        else
        {
            ImgGreen.localScale = new Vector3(0, ImgGreen.localScale.y, ImgGreen.localScale.z);
        }

    }
}
