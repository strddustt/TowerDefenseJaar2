using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Placement : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject particles;
    private TowerRuntimeStats stats;
    private GameObject preview;
    private SpriteRenderer previewSprite;
    PlacementCheck check;
    private Color spriteColor;
    private Camera cam;
    private bool dragging = false;
    private bool canPlace = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dragging)
            return;
        if (dragging)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
            preview.transform.position = pos;
        }
        if (preview != null && check.CheckPlacement())
        {
            setPlaceable(true);
        }
        else
        {
            setPlaceable(false);
        }
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            StartCoroutine(place());
        }

    }
    public void OnClick()
    {
        preview = new GameObject("new preview");
        check = preview.AddComponent<PlacementCheck>();
        stats = prefab.GetComponentInChildren<TowerRuntimeStats>();
        preview.AddComponent<CircleCollider2D>().radius = stats.footprintRadius;
        check.SetRadius(stats.footprintRadius);


        previewSprite = preview.AddComponent<SpriteRenderer>();
        previewSprite.sprite = prefab.gameObject.transform.root.GetComponent<SpriteRenderer>().sprite;
        spriteColor = previewSprite.color;
        spriteColor.a = 0.5f;
        spriteColor.b = 0;
        previewSprite.color = spriteColor;

        dragging = true;
    }
    public void setPlaceable(bool placeable)
    {
        spriteColor = placeable ? new Color(0f, 0.5f, 0f) : new Color(0.5f, 0f, 0f);
        previewSprite.color = spriteColor;
        canPlace = placeable;
    }
    public IEnumerator place()
    {
        if (canPlace)
        {
            dragging = false;
            GameObject tower = Instantiate(prefab, preview.transform.position, Quaternion.identity);
            Destroy(preview);
            GameObject particle = Instantiate(particles, tower.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.9f);
            Destroy(particle);
        }
    }
}
