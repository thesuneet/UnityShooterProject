using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance{get; set;}

    public AudioSource ShootingChannel;
   
    public AudioClip AK47_Shot;
    public AudioClip M4_Shot;

    public AudioSource ReloadingChannel;

    public AudioClip AK47_reload;
    public AudioClip M4_reload;

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

   public void PlayShootingSound(WeaponModel Weapon)
   {
    switch (Weapon)
    {
        case WeaponModel.AK47:
            ShootingChannel.PlayOneShot(AK47_Shot);
            break;

        case WeaponModel.M4:
            ShootingChannel.PlayOneShot(M4_Shot);
            break;


    }
   }

   public void PlayReloadSound(WeaponModel Weapon)
   {
    switch (Weapon)
    {
        case WeaponModel.AK47:
            ReloadingChannel.PlayOneShot(AK47_reload);
            break;

        case WeaponModel.M4:
            ReloadingChannel.PlayOneShot(M4_reload);
            break;
    }
   }

}
