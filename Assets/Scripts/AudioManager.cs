using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public float delay;

    private AudioSource source;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            if (source.volume >= 0.1f)
            {
                source.Play();
                timer = 0f;
            }
        }
    }
}
