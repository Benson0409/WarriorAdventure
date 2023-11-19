using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Treasure : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite clodeSprite;
    public bool isDone;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : clodeSprite;
    }
    public void TriggerAction()
    {
        Debug.Log("open");
        if (!isDone)
        {
            OpenTreasure();
        }
    }

    private void OpenTreasure()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";
    }

}
