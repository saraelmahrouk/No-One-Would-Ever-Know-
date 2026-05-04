using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1 : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
   public void PlayGame(){
      SceneManager.LoadSceneAsync(1);
      source.PlayOneShot(clip);
   }
}
