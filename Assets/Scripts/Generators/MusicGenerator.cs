using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class MusicGenerator : MonoBehaviour
{
    public List<AudioClip> samples;
    public List<AudioClip> shuffled;
    new AudioSource audio;

    public List<Note> tune;

    public class Note
    {
        public AudioClip clip;
        public float volume;
        public int skip = 1;

        public Note(AudioClip clip, float volume) {
            this.clip = clip;
            this.volume = volume;
        }
    }

    float next;
    int beat;

    Note RandomNote() {
        return UnityEngine.Random.Range(0, 1f) < 0.0f ? new Note(null, 0) : new Note(samples.rnd(), 1);
    }

    Note RandomNote(Note x) {
        if (x == null) {
            return null;
        }
        return UnityEngine.Random.Range(0, 1f) < 0.0f ? new Note(x.clip, 0) : new Note(samples.CyclicNext(x.clip, UnityEngine.Random.Range(-8, 9)), x.volume);
    }

    void Awake() {
        audio = GetComponent<AudioSource>();
        shuffled = samples.Shuffled();
        tune = new List<Note>();
        tune.Add(new Note(samples[(int)(samples.Count * UnityEngine.Random.Range(0.35f, 0.65f))], 1));
        for (int i = 0; i < 10; i++) {
            int n = tune.Count;
            for (int j = 0; j < n; j++) {
                if (UnityEngine.Random.Range(0,1f) < 0.2f) {
                    tune.Add(RandomNote(tune[j]));
                } else {
                    tune.Add(tune[j]);
                }
            }
            for (int j = 0; j < 10; j++) {
                int id = n + UnityEngine.Random.Range(0, n);
                tune[id] = RandomNote(tune[id]);
                if (UnityEngine.Random.Range(0, 1f) < 0.6f) {
                    break;
                }
            } 
            for (int j = 0; j < 10; j++) {
                int id = n + UnityEngine.Random.Range(0, n);
                if (UnityEngine.Random.Range(0, 1f) < Mathf.Pow(0.5f, Power(id))) {
                    tune[id].skip = (int)Mathf.Pow(2, Power(id));
                }
                if (UnityEngine.Random.Range(0, 1f) < 0.6f) {
                    break;
                }
            }
            //int delta = UnityEngine.Random.Range(-1, 2);
            //for (int j = 0; j < n; j++) {
            //    tune[n + j] = samples.CyclicNext(tune[n + j], delta);
            //}
        }
    }

    int Power(int beat) {
        int x = beat;
        int r = 0;
        while (x > 0 && x % 2 == 0) {
            x /= 2;
            r++;
        }
        return r;
    }

    int Index() {
        return beat / (2 << Power(beat));
    }

    void Update() {
        if (TimeManager.GameTime > next) {

            var sound = tune.Cyclic(beat);
            audio.PlayOneShot(sound.clip, sound.volume * (0.5f + Power(beat)*0.1f));
            //audio.pitch = Mathf.Pow(2, UnityEngine.Random.Range(-1, 1f));
            int skip = sound.skip;
            int powered = beat;
            while (powered % 2 == 0) {
                if (UnityEngine.Random.Range(0, 1f) < 1f) {
                    break;
                }
                powered /= 2;
                skip *= 2;
            }
            Debug.LogFormat("Playing {0}, skipping {1}", sound, skip);
            next = next + 0.25f * Mathf.Pow(0.999f, beat % 512 / 16 * 16) * skip;
            beat += skip;
        }
    }
}