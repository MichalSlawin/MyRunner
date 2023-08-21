using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _jumpClips;
    [SerializeField] private List<AudioClip> _loseClips;
    [SerializeField] private List<AudioClip> _slideClips;
    [SerializeField] private List<AudioClip> _startClips;
    [SerializeField] private List<AudioClip> _winClips;

    public void PlayClip(AudioClipType type)
    {
        switch (type)
        {
            case AudioClipType.Jump:
                PlayRandomClip(_jumpClips);
                break;
            case AudioClipType.Lose:
                PlayRandomClip(_loseClips);
                break;
            case AudioClipType.Slide:
                PlayRandomClip(_slideClips);
                break;
            case AudioClipType.Start:
                PlayRandomClip(_startClips);
                break;
            case AudioClipType.Win:
                PlayRandomClip(_winClips);
                break;
        }
    }

    private void PlayRandomClip(List<AudioClip> clips)
    {
        System.Random random = new System.Random();
        int clipNum = random.Next(clips.Count);

        _audioSource.clip = clips[clipNum];
        _audioSource.Play();
    }
}
