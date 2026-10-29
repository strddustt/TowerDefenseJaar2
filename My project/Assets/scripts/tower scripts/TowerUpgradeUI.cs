using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject UIPrefab;
    private GameObject UI;
    [SerializeField] private UpgradeSO upgrade;
    private bool UICloseFlag = true;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && UICloseFlag == false)
        {
            Destroy(UI);
            UI = null;
        }
    }
    private void OnMouseDown()
    {
        if (UI != null)
        {
            UI = Instantiate(UIPrefab);
        }
    }

    public void KeepUIOpen()
    {
        UICloseFlag = false;
    }
}
