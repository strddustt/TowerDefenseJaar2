using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    private UpgradeSO upgrades;
    public static event Action<int> upgraded;
    public int upgradeIndex { get; private set; }
    private TowerRuntimeStats stats;
    private TextMeshProUGUI text = new TextMeshProUGUI();
    private TextMeshProUGUI activeText;
    // Start is called before the first frame update
    public void Init(TowerRuntimeStats givenStats, UpgradeSO givenUpgrades, int currentIndex)
    {
        upgrades = givenUpgrades;
        stats = givenStats;
        upgradeIndex = currentIndex;
        activeText = Instantiate(text, transform);
        Vector2 pos = activeText.rectTransform.anchoredPosition;
        pos.y -= 30;
        activeText.rectTransform.anchoredPosition = pos;
        if (upgradeIndex == 0)
            activeText.text = upgrades.name;
        else
            activeText.text = upgrades.Upgrades[upgradeIndex].name;
    }


    private void OnPress()
    {
        if (Cah.cash > upgrades.Upgrades[upgradeIndex].cost)
        {
            upgradeIndex++;
            upgraded?.Invoke(upgradeIndex);
            activeText.text = upgrades.Upgrades[upgradeIndex].name;
        }
    }
}
