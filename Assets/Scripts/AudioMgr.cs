using UnityEngine;
using System.Collections;

public enum SoundEffectType
{
   kHappyEnd,
   kSadEnd,
   kBabyCoo,
   kBabyLaugh,
   kChewingCrunch,
   kChewingSoft,
   kDrinking,
   kVomit
}

public class AudioMgr : MonoBehaviour {

   public static AudioMgr Instance;

   public AudioClip[] tracks;
   public AudioClip[] sfx;
   public AudioSource source;

   void Awake()
   {
      Instance = this;
      source.clip = tracks[0];
   }

   public void StopTrack()
   {
      source.Stop();
   }

   public void PlayBGM(Stage stage)
   {
      source.clip = tracks[(int)stage];
      source.Play();
   }

   public void PlaySFX(SoundEffectType sfxType)
   {
      AudioSource.PlayClipAtPoint(sfx[(int)sfxType], transform.position);
   }
}
