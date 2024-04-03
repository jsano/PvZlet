using System.Collections;
using System.Collections.Generic;
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

    public AudioClip song;
    public AudioClip rise;

    // Update is called once per frame
    public override void Update()
    {
        if (transform.position.x > Tile.COL_TO_WORLD[8] && intro) WalkConstant();
        else
        {
            if (intro) SFX.Instance.Play(song, true);
            walkTime = alternateWalkTime[0];
            intro = false;
        }
        if (!intro)
        {
            List<string> missing = new List<string>();
            if (up == null && row > 1 && !Tile.tileObjects[row - 1, 1].water) missing.Add("up");
            if (down == null && row < ZombieSpawner.Instance.lanes && !Tile.tileObjects[row + 1, 1].water) missing.Add("down");
            if (right == null) missing.Add("right");
            if (left == null) missing.Add("left");
            if (!isEating() && missing.Count > 0) spawnPeriod += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            if (spawnPeriod > spawnDelay)
            {
                SFX.Instance.Play(rise);
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
            ResetWalk();
            if (up != null) up.ResetWalk();
            if (down != null) down.ResetWalk();
            if (right != null) right.ResetWalk();
            if (left != null) left.ResetWalk();
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
                up.waveNumber = waveNumber;
            }
            if (s == "down")
            {
                down = Instantiate(backup).GetComponent<Backup>();
                if (hypnotized) down.Hypnotize();
                down.row = row + 1;
                down.xLoc = transform.position.x;
                down.waveNumber = waveNumber;
            }
            if (s == "right")
            {
                right = Instantiate(backup).GetComponent<Backup>();
                if (hypnotized) right.Hypnotize();
                right.row = row;
                right.xLoc = transform.position.x + Tile.TILE_DISTANCE.x;
                right.waveNumber = waveNumber;
            }
            if (s == "left")
            {
                left = Instantiate(backup).GetComponent<Backup>();
                if (hypnotized) left.Hypnotize();
                left.row = row;
                left.xLoc = transform.position.x - Tile.TILE_DISTANCE.x;
                left.waveNumber = waveNumber;
            }
        }
        ZombieSpawner.Instance.SubtractBuild(-missing.Count, waveNumber);
        StartCoroutine(FinishSpawn());
    }

    private IEnumerator FinishSpawn()
    {
        yield return new WaitForSeconds(1.5f);
        spawning = false;
        ResetWalk();
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
