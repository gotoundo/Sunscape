using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager Main;
    public AudioClip[] AbilityEffects;
   // AudioSource source;

    
    
    void Awake()
    {
        Main = this;
    }

    public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
