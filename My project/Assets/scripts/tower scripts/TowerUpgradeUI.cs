using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject UIPrefab;
    private GameObject UI;
    private TowerRuntimeStats stats;
    [SerializeField, Tooltip("sort paths numbers from left to right")] private UpgradeSO[] upgrade = new UpgradeSO[3];
    private bool UICloseFlag = true;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<TowerRuntimeStats>(); 
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && UICloseFlag == false && UI != null)
        {
            UI.SetActive(false);
        }
        UICloseFlag = false;
    }
    private void OnMouseDown()
    {
        Debug.Log("detected click");
        UICloseFlag = true;
        if (UI == null)
        {
            UI = Instantiate(UIPrefab);
            UpgradeButton[] buttons = new UpgradeButton[3];
            foreach (var button in UI.GetComponentsInChildren<UpgradeButton>())
            {
                for (int i =0; i < buttons.Length; i++)
                {
                    if (buttons[i].gameObject.transform.position.x < button.gameObject.transform.position.x && buttons[i] != null)
                    {
                        UpgradeButton buttonToReplace = buttons[i];
                        buttons[i] = button;
                        buttons[i + 1] = buttonToReplace;
                    }
                    else
                        buttons[i] = button;
                }
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Init(stats, upgrade[i]);
            }
        }
        if (UI != null)
            UI.SetActive(true);
    }

    public void KeepUIOpen()
    {
        UICloseFlag = true;
    }
}
