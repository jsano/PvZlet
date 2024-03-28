using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Zombie
{

    public float noLadderWalkTime;

    public Sprite placedLadderSprite;

    // Update is called once per frame
    public override void Update()
    {
        if (shield == null) walkTime = noLadderWalkTime;
        else
        {
            GameObject p = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position, Vector3.one, 0, Vector2.zero, 0, LayerMask.GetMask("Plant")));
            if (p != null && p.GetComponent<Nut>() != null)
            {
                shield.transform.SetParent(p.transform, true);
                shield.transform.localPosition = new Vector3(0.3f, 0, 0);
                shield.layer = LayerMask.NameToLayer("ExplosivesOnly");
                shield.GetComponent<SpriteRenderer>().sprite = placedLadderSprite;
                shield.GetComponent<SpriteRenderer>().sortingLayerName = "Zombie";
                shield.GetComponent<SpriteRenderer>().sortingOrder = -1;
                shield.GetComponent<Shield>().enabled = false;
                shield = null;
            }
        }
        base.Update();
    }

}
