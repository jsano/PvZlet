using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CobCannon : Plant
{

    public float reload;
    private float reloadPeriod;
    private bool aiming;
    private bool ready = true;
    private Camera cam;

    public GameObject explosion;
    public Vector2 area;

    public override void Start()
    {
        base.Start();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!ready) reloadPeriod += Time.deltaTime;
        if (reloadPeriod >= reload)
        {
            ready = true;
            SR.material.color = Color.white;
        }
        if (aiming)
        {
            if (Input.GetMouseButtonDown(1)) aiming = false;
            if (Input.GetMouseButtonDown(0))
            {
                aiming = false;
                ready = false;
                reloadPeriod = 0;
                StartCoroutine(Update_Helper(cam.ScreenToWorldPoint(Input.mousePosition)));
            }
        }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.currentSelectedGameObject == null && !aiming && ready) StartCoroutine(AimDelay());
    }

    private IEnumerator AimDelay()
    {
        yield return new WaitForEndOfFrame();
        aiming = true;
    }

    private IEnumerator Update_Helper(Vector3 loc)
    {
        yield return new WaitForSeconds(atkspd);
        SR.material.color -= Color.white / 2;
        GameObject p = Instantiate(projectile, new Vector2(loc.x, loc.y + Tile.TILE_DISTANCE.y * 10), Quaternion.identity);
        while (p.transform.position.y > loc.y)
        {
            p.transform.Translate(Vector3.down * 15 * Time.deltaTime);
            yield return null;
        }
        GameObject g = Instantiate(explosion, p.transform.position, Quaternion.identity);
        g.transform.localScale = area * Tile.TILE_DISTANCE;
        RaycastHit2D[] all = Physics2D.BoxCastAll(p.transform.position, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage, gameObject);
        }
        Destroy(p);
    }

}
