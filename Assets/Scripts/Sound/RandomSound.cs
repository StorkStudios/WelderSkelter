using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] mySounds;

    private AudioClip activeSound;

    // Update is called once per frame
    void Update()
    {
        // if I press the spacebar, play a sound
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // set active sound
            // Uwaga: To zadzia³a tylko jeœli lista mySounds nie jest pusta!
            if (mySounds.Length > 0)
            {
                activeSound = mySounds[Random.Range(0, mySounds.Length)];

                // play sound
                audioSource.PlayOneShot(activeSound);
            }
        }
    }
}