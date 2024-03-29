using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType EitemType;
    public float rate;

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        EitemType = data.itemType;
        rate = data.damages[0];

        ApplyGear();
    }

    public void Levelup(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (EitemType)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
            default:
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case (int)ItemData.ItemType.Melee:
                    weapon.speed = 150 + (150 * rate);
                    break;
                default:
                    weapon.speed = 0.3f * (1f - rate);
                    break;
            }
        }
    }
    void SpeedUp()
    {
        float speed = 3f;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
