using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class SetUpgradePos
{
    private static readonly Vector3[] corners = new Vector3[4];

    public static void MoveLeft(RectTransform target, Button button, TMP_Text label)
    {
        RectTransform buttonRt = (RectTransform)button.transform;
        RectTransform labelRt = label.rectTransform;
        RectTransform slotRt = (RectTransform)buttonRt.parent;   
        RectTransform panelRt = (RectTransform)target.parent;    

        float labelHeight = labelRt.rect.height;

        target.GetWorldCorners(corners);
        Vector3 topLeft = slotRt.InverseTransformPoint(corners[1]);
        Vector3 bottomRight = slotRt.InverseTransformPoint(corners[3]);
        float width = bottomRight.x - topLeft.x;
        float height = topLeft.y - bottomRight.y;
        float x = topLeft.x - slotRt.rect.xMin;
        float y = topLeft.y - slotRt.rect.yMax;

        SetTopLeft(buttonRt);
        buttonRt.anchoredPosition = new Vector2(x, y);
        buttonRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        buttonRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        panelRt.GetWorldCorners(corners);
        float panelLeft = slotRt.InverseTransformPoint(corners[0]).x;
        float panelRight = slotRt.InverseTransformPoint(corners[2]).x;
        float gap = (panelRight - panelLeft) * 0.05f;
        float remaining = panelRight - bottomRight.x - gap;

        SetTopLeft(labelRt);
        labelRt.anchoredPosition = new Vector2(x + width + gap, y);
        labelRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, remaining * 0.5f);
        labelRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, labelHeight);
        label.alignment = TextAlignmentOptions.TopLeft;
    }

    private static void SetTopLeft(RectTransform rt)
    {
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
    }
}
