using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance{get; set;}

    public AudioSource shootingSoundAK_47;
    public AudioSource reloadSoundAK_47;
    public AudioSource emptyMagazineSoundAK_47;
    
   private void Awake()
   {
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
    }
    else
    {
        Instance = this;
    }
   }
}
