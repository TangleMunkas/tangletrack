using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioClip clickSound;
    public AudioClip magicSound;
    public AudioClip hornSound;
    public AudioClip levelCompleteSound;
    public AudioClip buyingSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    public void PlayClick() => PlaySound(clickSound);
    public void PlayMagic() => PlaySound(magicSound);
    public void PlayHorn() => PlaySound(hornSound);
    public void PlayLevelComplete() => PlaySound(levelCompleteSound);
    public void PlayBuying() => PlaySound(buyingSound);
}
