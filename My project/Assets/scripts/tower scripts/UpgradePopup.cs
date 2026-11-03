using UnityEngine;

/// <summary>
/// Put this on the root of the popup prefab (which also has a World Space Canvas).
/// Handles the "pick one path, others disappear" behaviour.
/// </summary>
public class UpgradePopup : MonoBehaviour
{
    [SerializeField, Tooltip("The 3 upgrade slots, ordered left to right")]
    private UpgradeButton[] slots = new UpgradeButton[3];
    [SerializeField, Tooltip("Empty RectTransform (sibling of the slots) marking where the chosen slot moves to")]
    private RectTransform chosenAnchor;
    [SerializeField, Tooltip("Width the description text gets once a path is chosen")]
    private float expandedLabelWidth = 300f;

    private bool pathChosen;

    public void Init(TowerRuntimeStats stats, UpgradeSO[] paths)
    {
        Canvas canvas = GetComponentInChildren<Canvas>(true);
        if (canvas != null)
            canvas.worldCamera = Camera.main;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < paths.Length && paths[i] != null)
                slots[i].Init(this, stats, paths[i]);
            else
                slots[i].gameObject.SetActive(false);
        }
    }

    /// <summary>Called by a slot after its first purchase.</summary>
    public void PathChosen(UpgradeButton chosen)
    {
        if (pathChosen)
            return;
        pathChosen = true;

        foreach (UpgradeButton slot in slots)
        {
            if (slot != chosen)
                slot.gameObject.SetActive(false);
        }

        chosen.MoveToLeft(chosenAnchor, expandedLabelWidth);
    }
}