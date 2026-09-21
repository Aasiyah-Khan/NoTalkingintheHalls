using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{
    //Public list for AudioSources
    public List<AudioSource> randomSound = new List<AudioSource>();

    //A time delay of seconds
    public float delayTime = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("List Count: " + randomSound.Count);

        StartCoroutine(PlaySoundAfterDelay());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    IEnumerator PlaySoundAfterDelay()
    {

        while (true)
        {
            yield return new WaitForSeconds(delayTime);

            if (randomSound != null && randomSound.Count > 0)
            {
                //Picks a random number from the list
                int randomIndex = Random.Range(0, randomSound.Count);

                //Gets the AudioSource from that number
                AudioSource chosenAudioSource = randomSound[randomIndex];

                //Plays that chosen AudioSource
                if (chosenAudioSource != null)
                {
                    chosenAudioSource.Play();
                }
                else
                {
                    Debug.LogWarning("Chosen AudioSource was destroyed before playing");
                }
            }
            else
            {
                yield break;
            }
        }
    }
}
