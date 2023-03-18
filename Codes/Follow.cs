using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        Vector3 rectPos = GameManager.instance.player.transform.position;
        rectPos.y = rectPos.y - 0.8f;
        rect.position = Camera.main.WorldToScreenPoint(rectPos);
        
    }
}
