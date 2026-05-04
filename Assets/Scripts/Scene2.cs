using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2 : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
   public void NextRoom(){
      SceneManager.LoadSceneAsync(0);
      source.PlayOneShot(clip);
   }
}
