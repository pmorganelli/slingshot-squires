using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    public Sprite icon;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Player player = collision.GetComponent<Player>();

        if(player)
        {
            player.inventory.Add(this);
            Destroy(this.gameObject);
        }
    }
}

public enum CollectableType
{
    NONE,
    APPLE_SEED,
    CARROT_SEED,
    PUMPKIN_SEED,
    WATERMELON_SEED,
    GOLD_BALL,
    DIAMOND_BALL 
}
