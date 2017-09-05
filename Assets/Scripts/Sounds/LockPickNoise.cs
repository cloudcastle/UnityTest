using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;

public class LockPickNoise : MonoBehaviour
{
    public Player player;
    public AudioSource noise;

    List<CleanHands> cleanHands;

    public void Start() {
        cleanHands = FindObjectsOfType<CleanHands>().ToList();
        cleanHands.ForEach(c => c.onStayAtDistance.AddListener(new UnityAction<float>(OnStayInCleanHandsAtDistance)));
    }

    public void OnStayInCleanHandsAtDistance(float distance) {
        noise.volume = Mathf.Max(noise.volume, 0.3f * (1-distance));
    }

    public void FixedUpdate() {
        noise.volume = noise.volume * Mathf.Pow(0.5f, Time.fixedDeltaTime * 20);
        //float distance = cleanHands.ExtMin(c => c.Distance(player.current));
        //noise.volume = Mathf.Pow(0.5f, distance);
//        if (player.current.inventory.pickStun.OnCooldown()) {
//            noise.volume = 0.2f;
//        } else {
//            noise.volume = 0;
//        }
    }
}