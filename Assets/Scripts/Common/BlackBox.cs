using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBox : MonoBehaviour
{
    float horizonScale, verticalScale;

    void Start()
    {
        horizonScale = (float)Screen.width / 1080;
        verticalScale = (float)Screen.height / 1920;

        switch (name)
        {
            case "BlackBox_Left":
                HorizonBox();
                break;
            case "BlackBox_Right":
                HorizonBox();
                break;
            case "BlackBox_Up":
                VerticalBox();
                break;
            case "BlackBox_Down":
                VerticalBox();
                break;
        }
    }

    void HorizonBox()
    {
        if (horizonScale > verticalScale)
        {
            float width = (float)(Screen.width / (verticalScale * 2f)) - 540f;
            float height = Screen.height / verticalScale;

            RectTransform rectTran = GetComponent<RectTransform>();
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width * 2f);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }

    void VerticalBox()
    {
        if (horizonScale < verticalScale)
        {
            float width = Screen.width / horizonScale;
            float height = (float)(Screen.height / (horizonScale * 2f)) - 960f;

            RectTransform rectTran = GetComponent<RectTransform>();
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height * 2f);
        }
    }
}
