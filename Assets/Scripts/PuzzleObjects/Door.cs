using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(Colored))]
public class Door : MonoBehaviour
{
    KeyColor oldKeyColor;
    public KeyColor keyColor;
    Color opened;
    Color closed;
    float openedAlpha = 0.25f;
    float closedAlpha = 0.75f;

    Colored colored;

    void Awake() {
        colored = GetComponent<Colored>();
    }

    void Start() {
        if (!Extensions.Editor()) {
            keyColor.doors.Add(this);
        }
    }

    public bool Opened(Player player) {
        return keyColor.openedFor.Contains(player);
    }

    public void OpenFor(Player player, bool open = true) {
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), open);
    }

    void Update() {
        if (Extensions.Editor()) {
            if (keyColor != oldKeyColor) {
                oldKeyColor = keyColor;
                closed = opened = keyColor.color;
                opened.a = openedAlpha;
                closed.a = closedAlpha;
            }
            colored.color = closed;
        } else {
            GetComponent<MeshRenderer>().material.ChangeAlpha(Opened(Player.current) ? openedAlpha : closedAlpha);
        }
    }
}
