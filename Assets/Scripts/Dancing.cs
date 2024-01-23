using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Dancing : Zombie
{

    public GameObject backup;
    private Backup up;
    private Backup down;
    private Backup left;
    private Backup right;
    public float spawnDelay;
    private float spawnPeriod = 999;
    private bool spawning = false;

    private bool intro = true;
    public float dancingWalkTime;

    private ZombieSpawner s;

    // Update is called once per frame
    public override void Update()
    {
        if (transform.position.x > Tile.COL_TO_WORLD[7] && intro) WalkConstant();
        else
        {
            s = GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>();
            walkTime = dancingWalkTime;
            intro = false;
        }
        if (!intro)
        {
            List<string> missing = new List<string>();
            if (up == null && row > 1) missing.Add("up");
            if (down == null && row < s.lanes) missing.Add("down");
            if (right == null) missing.Add("right");
            if (left == null) missing.Add("left");
            if (!isEating() && missing.Count > 0) spawnPeriod += Time.deltaTime;
            if (spawnPeriod > spawnDelay)
            {
                spawnPeriod = 0;
                spawning = true;
                SpawnBackups(missing);
            }
            if (spawning == false) base.Update();
        }
    }

    public override void LateUpdate()
    {
        if (up != null && up.isEating() ||
            down != null && down.isEating() ||
            right != null && right.isEating() ||
            left != null && left.isEating() ||
            isEating() || spawning)
        {
            RB.velocity = Vector3.zero;
            period = 0;
            if (up != null) up.StopForOthers();
            if (down != null) down.StopForOthers();
            if (right != null) right.StopForOthers();
            if (left != null) left.StopForOthers();
        }
        base.LateUpdate();
    }

    private void SpawnBackups(List<string> missing)
    {
        foreach (string s in missing)
        {
            if (s == "up")
            {
                up = Instantiate(backup).GetComponent<Backup>();
                if (hypnotized) up.Hypnotize();
                up.row = row - 1;
                up.xLoc = transform.position.x;
            }
            if (s == "down")
            {
                down = Instantiate(backup).GetComponent<Backup>();
                if (hypnotized) down.Hypnotize();
                down.row = row + 1;
                down.xLoc = transform.position.x;
            }
            if (s == "right")
            {
                right = Instantiate(backup).GetComponent<Backup>();
                if (hypnotized) right.Hypnotize();
                right.row = row;
                right.xLoc = transform.position.x + Tile.TILE_DISTANCE.x;
            }
            if (s == "left")
            {
                left = Instantiate(backup).GetComponent<Backup>();
                if (hypnotized) left.Hypnotize();
                left.row = row;
                left.xLoc = transform.position.x - Tile.TILE_DISTANCE.x;
            }
        }
        StartCoroutine(FinishSpawn());
    }

    private IEnumerator FinishSpawn()
    {
        yield return new WaitForSeconds(1.5f);
        spawning = false;
    }

    public override void Hypnotize()
    {
        if (up != null) up.Hypnotize();
        if (down != null) down.Hypnotize();
        if (right != null) right.Hypnotize();
        if (left != null) left.Hypnotize();
        base.Hypnotize();
    }

}
