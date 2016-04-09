﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class KeyColor : MonoBehaviour
{
    public Color color;

    public List<Door> doors = new List<Door>();
    public List<Key> keys = new List<Key>();

    public HashSet<Player> openedFor = new HashSet<Player>();

    void OpenFor(Player player, bool open = true) {
        if (openedFor.Contains(player) == open) {
            return;
        }
        doors.ForEach(door => door.OpenFor(player, open));
        Debug.Log(string.Format("Color {0} open for {1} : {2}", this, player, open));
        if (open) {
            openedFor.Add(player);
        } else {
            openedFor.Remove(player);
        }
    }

    void Start() {
        FindObjectOfType<Undo>().onUndo += OnUndo;
    }

    public void Recalculate(Player player) {
        if (player.inventory.items.Any(item => item is Key && (item as Key).keyColor == this)) {
            OpenFor(player, true);
        } else {
            OpenFor(player, false);
        }
    }

    public void OnUndo() {
        Player.all.ForEach(Recalculate);
    }
}
