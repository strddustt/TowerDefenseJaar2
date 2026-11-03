using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Put this on the tower. Detects clicks on the tower's click collider with RaycastAll
/// (so the footprint collider can't block it) and opens/closes the upgrade popup above the tower.
/// </summary>
public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private UpgradePopup popupPrefab;
    [SerializeField] private Vector3 popupOffset = new Vector3(0f, 1.5f, 0f);
    [SerializeField, Tooltip("The collider that counts as 'clicking the tower'. Falls back to the first Collider on this object.")]
    private Collider2D clickCollider;
    [SerializeField, Tooltip("Upgrade paths, sorted left to right (same order as the slots in the popup)")]
    private UpgradeSO[] upgrade = new UpgradeSO[3];

    private TowerRuntimeStats stats;
    private UpgradePopup popup;
    private Camera cam;

    private void Start()
    {
        stats = GetComponentInChildren<TowerRuntimeStats>();
        cam = Camera.main;
        if (clickCollider == null)
            clickCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        // Click landed on a UI element (e.g. an upgrade button): not a world click.
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (ClickedThisTower())
            OpenPopup();
        else
            ClosePopup();
    }

    private bool ClickedThisTower()
    {
        Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        return clickCollider.OverlapPoint(worldPoint);
    }

    private void OpenPopup()
    {
        Vector3 pos = transform.position + popupOffset;
        if (popup == null)
        {
            popup = Instantiate(popupPrefab, pos, cam.transform.rotation);
            popup.Init(stats, upgrade);
        }
        popup.transform.position = pos;
        popup.gameObject.SetActive(true);
    }

    private void ClosePopup()
    {
        if (popup != null)
            popup.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (popup != null)
            Destroy(popup.gameObject);
    }
}