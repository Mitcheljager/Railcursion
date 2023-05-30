using UnityEngine;
using UnityEngine.Audio;

public class AudioVolume : MonoBehaviour {
    public AudioMixer mixer;

    void Start() {
        SetVolume("MasterVolume");
    }

    public void SetVolume(string key) {
        float volume = PlayerPrefs.GetFloat(key, 80f) / 100f;
        if (volume == 0f) volume = 0.00001f;

        float log10volume = Mathf.Log10(volume) * 20;

        mixer.SetFloat(key, log10volume);
    }
}
