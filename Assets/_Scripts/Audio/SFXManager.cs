using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    fuego,
    moneda,
    CaminarI,
    caminarD,
    e_ehh,
    e_hmm,
    e_oh
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager SI;

    private void Awake()
    {
        SI = SI == null ? this : SI;
    }

    //Referencias a los audios source respectivos
    [SerializeField] private AudioSource fuego, moneda, caminarD, CaminarI, e_ehh, e_hmm, e_oh;

    public void PlaySound(Sound soundToPlay)
    {
        switch (soundToPlay)
        {
            case Sound.fuego:
                fuego.PlayOneShot(fuego.clip);
                break;
            case Sound.caminarD:
                caminarD.PlayOneShot(caminarD.clip);
                break;
            case Sound.CaminarI:
                CaminarI.PlayOneShot(CaminarI.clip);
                break;
            case Sound.moneda:
                moneda.PlayOneShot(moneda.clip);
                break;
            case Sound.e_ehh:
                e_ehh.PlayOneShot(e_ehh.clip);
                break;
            case Sound.e_hmm:
                e_hmm.PlayOneShot(e_hmm.clip);
                break;
            case Sound.e_oh:
                e_oh.PlayOneShot(e_oh.clip);
                break;
        }
    }
}