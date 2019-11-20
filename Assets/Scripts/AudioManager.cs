using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource source;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<EnnemyControl>().ennemyObject.GetComponent<AudioSource>();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= VariableManager.variableManager.delayBetweenSoundFantome)
        {
            if (source.volume >= 0f)
            {
                source.Play();
                timer = 0f;
            }
        }
    }
}
