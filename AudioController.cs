using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;
    private float defaultVolume;
    private static AudioController _instance;
    private static AudioController Instance { get { return _instance; } }
    Coroutine increaseVol;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        defaultVolume = audioSource.volume;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Title Menu Screen")
        {
            increaseVol = StartCoroutine(IncreaseVolumeGradually());
        }
        else { return; }
    }

    public void DecreaseVolume()
    {
        var currentVolume = audioSource.volume;
        if(increaseVol != null)
        {
            StopCoroutine(increaseVol);
        }
        StartCoroutine(DecreaseVolumeGradually(currentVolume));
    }

    IEnumerator DecreaseVolumeGradually(float currentVolume)
    {
        var newVolume = currentVolume;
        while(newVolume > 0f)
        {
            newVolume -= 0.05f;
            audioSource.volume = newVolume;
            yield return new WaitForSeconds(0.25f);
        }
        audioSource.enabled = false;
        yield break;
    }

    IEnumerator IncreaseVolumeGradually()
    {
        audioSource.enabled = true;
        var newVolume = audioSource.volume;
        while(newVolume < defaultVolume)
        {
            newVolume += 0.05f;
            audioSource.volume = newVolume;
            yield return new WaitForSeconds(0.25f);
        }

    }
}
