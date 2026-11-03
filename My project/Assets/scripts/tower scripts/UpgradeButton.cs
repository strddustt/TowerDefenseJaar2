using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;

    public static event Action<int> upgraded;
    [SerializeField] private TMP_Text costText; 
    public int upgradeIndex => tier;

    private UpgradePopup popup;
    private TowerRuntimeStats stats;
    private UpgradeSO path;
    private int tier; 

    public void Init(UpgradePopup givenPopup, TowerRuntimeStats givenStats, UpgradeSO givenPath)
    {
        popup = givenPopup;
        stats = givenStats;
        path = givenPath;
        tier = 0;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnPress);
        Refresh();
    }

    private void Update()
    {
        // Grey the button out while the player can't afford the next tier.
        if (path != null && tier < path.Upgrades.Count)
            button.interactable = Cah.cash >= path.Upgrades[tier].cost;
    }

    private void OnPress()
    {
        if (tier >= path.Upgrades.Count)
            return;

        UpgradeStats next = path.Upgrades[tier];
        if (Cah.cash < next.cost)
            return;

        Cah.AddCash(-(int)next.cost);
        ApplyUpgrade.Apply(next, stats);
        tier++;
        upgraded?.Invoke(tier);

        popup.PathChosen(this);
        Refresh();
    }

    private void Refresh()
    {
        if (tier >= path.Upgrades.Count)
        {
            label.text = "Maxed";
            costText.text = "";
            button.interactable = false;
            return;
        }

        UpgradeStats next = path.Upgrades[tier];
        label.text = next.name;
        costText.text = "$" + next.cost;
    }

    public void MoveToLeft(RectTransform target, float labelWidth)
    {
        SetUpgradePos.MoveLeft(target, button, label);
    }
}