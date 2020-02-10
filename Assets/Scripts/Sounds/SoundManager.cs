using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	
	public Sound[] sounds;
	
	public static SoundManager instance;

    private string sceneName;
	
    void Awake() {
		
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
			return;	
		}
		
		DontDestroyOnLoad(gameObject);
		
		foreach (Sound s in sounds) {
			
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;

            s.source.playOnAwake = false;
			
		}
	} 
	
	IEnumerator Start() {
		
		Play("Ambience");
		
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        
        //Tutorial Erklärungsablauf
        if (sceneName == "Tutorial Laser" || sceneName == "Tutorial Bubble") {
	        
	        yield return new WaitForSeconds(3);
	        Play("Tut Intro");
	        yield return new WaitForSeconds(16);

	        if (sceneName == "Tutorial Laser") {
		        Play("Tut Laser");
		        yield return new WaitForSeconds(8);
	        } else {
		        Play("Tut Bubble");
		        yield return new WaitForSeconds(15);
	        }
	        
	        Play("Tut Desel");
	        yield return new WaitForSeconds(14);
        
	        Play("Tut Nav");
        }
	}
	
   

    public void Play(string name) {     

		Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("gibbet nich den sound");
            return;
        }
        
        s.source.Play();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("gibbet nich den sound");
            return false;
        }

        return s.source.isPlaying;
    }
}
