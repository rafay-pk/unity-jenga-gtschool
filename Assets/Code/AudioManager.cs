using System;
using UnityEngine;

namespace Code
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip music, blockSelect;
        [SerializeField] private AudioSource musicPlayer;

        private void Start()
        {
            musicPlayer.clip = music;
        }
    }
}
