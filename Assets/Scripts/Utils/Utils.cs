using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    private static GameObject mainCameraCache;

    public static void PlayAllParticleSystemFrom(GameObject particleGameObject, float scale, bool detachFromParent, float destroyDelay)
    {
        ParticleSystem part = particleGameObject.GetComponent<ParticleSystem>();
        if (part != null)
        {
            ParticleSystem.MainModule main = part.main;
            main.startSize = scale;
            part.Play();
            if (detachFromParent)
            {
                particleGameObject.transform.parent = null;
            }
        }
        GameObject.Destroy(particleGameObject, destroyDelay);

        //Recursive call to play particles on childs
        foreach (Transform child in particleGameObject.transform)
        {
            PlayAllParticleSystemFrom(child.gameObject, scale, detachFromParent, destroyDelay);
        }
    }

    public static GameObject GetMainCamera()
    {
        //To Find the camera only once
        if(mainCameraCache == null)
        {
            mainCameraCache = GameObject.FindGameObjectWithTag("MainCamera");
        }

        return mainCameraCache;
    }

    public static AudioClip GetRandomClip(List<AudioClip> audioClips)
    {
        if (audioClips != null && audioClips.Count > 0)
        {
            return audioClips[Random.Range(0, audioClips.Count)];
        }
        else
        {
            return null;
        }
    }

}
