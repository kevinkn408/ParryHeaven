using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    AudioController audioController;
    // Start is called before the first frame update
    private void Awake()
    {
        audioController = FindObjectOfType<AudioController>();
    }


    void Start()
    {
        sceneLoader.OnSceneChange += audioController.DecreaseVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
