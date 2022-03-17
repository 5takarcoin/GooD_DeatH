using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string[] sentences;
    public float typingSpeed;

    bool writing;

    public bool trigger;
    public Color tutorial = new Color ( 49, 249, 255, 255 );
    public Color narration = new Color(255, 249, 49, 255);

    bool done;

    void Update()
    {
        if (!done && Check())
        {
            trigger = true;
            done = true;
        }
    }

    
    bool Check()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, transform.rotation);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Focus")
            {
                return true;
            }
        }
        return false;
    }
}
