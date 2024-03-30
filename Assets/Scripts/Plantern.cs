using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantern : Plant
{

    public AudioClip start;

    public override void Start()
    {
        SFX.Instance.Play(start);
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        for (int i = Mathf.Max(1, row - 2); i <= Mathf.Min(row + 2, ZombieSpawner.Instance.lanes); i++)
            for (int j = Mathf.Max(1, col - 2); j <= Mathf.Min(col + 2, 9); j++)
            {
                if (Tile.tileObjects[i, j].fog != null) Tile.tileObjects[i, j].fog.Clear();
            }
        base.Update();
    }

    protected override void Attack(Zombie z)
    {
        for (int i = Mathf.Max(1, row - 1); i <= Mathf.Min(row + 1, ZombieSpawner.Instance.lanes); i++)
            for (int j = Mathf.Max(1, col - 1); j <= Mathf.Min(col + 1, 9); j++)
                if (Tile.tileObjects[i, j].GetEatablePlant() != null) Tile.tileObjects[i, j].GetEatablePlant().GetComponent<Plant>().Heal(1);
        base.Attack(z);
    }

}
