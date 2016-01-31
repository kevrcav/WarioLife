using UnityEngine;
using System.Collections;

public class DangerSounds : MonoBehaviour {
    public void OnHitByCarSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kHitByCar);
    }
    public void OnSlowCarSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kCarAccelSlow);
    }
    public void OnFastCarSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kCarAccelFact);
    }
    public void OnTireSquealSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kTireSqual);
    }
    public void OnZapSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kZap);
    }
    public void OnDingSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kDing);
    }
    public void OnCrunchSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kChewingCrunch);
    }
    public void OnWhooshSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kWhoosh);
    }
    public void OnCrowdSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kCrowdAww);
    }
}
