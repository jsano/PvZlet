using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{

    private enum State
    {
        Clearing,
        Showing
    }

    private SpriteRenderer SR;
    private float alphaSpeed = 2;
    private float showStartup = 0.25f;
    private float period;
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        state = State.Showing;
        SR.color *= new Color(1, 1, 1, 0);
    }

    void Update()
    {
        period = Mathf.Max(0, period - Time.deltaTime);
        if (period == 0) state = State.Showing;
        if (state == State.Showing)
        {
            if (SR.color.a < 0.99f) SR.color += new Color(0, 0, 0, alphaSpeed * Time.deltaTime);
        }
        else if (SR.color.a > 0) SR.color -= new Color(0, 0, 0, alphaSpeed * Time.deltaTime);
    }

    public void Clear(float duration = 0)
    {
        if (duration == 0)
        {
            if (period <= showStartup) period = showStartup;
        }
        else period = duration;
        state = State.Clearing;
    }

}
