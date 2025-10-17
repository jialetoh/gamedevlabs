using UnityEngine;

public class Coin : MonoBehaviour
{
    public Animator coinAnimator;
    public AudioSource coinAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void PlayCoinSound()
    {
        coinAudio.PlayOneShot(coinAudio.clip);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
