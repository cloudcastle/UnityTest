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

    public HashSet<Player> openedFor = new HashSet<Player>();

    void Awake() {
        colored = GetComponent<Colored>();
    }

    void Start() {
#if UNITY_EDITOR
        if (!Extensions.Editor()) {
            keyColor.doors.Add(this);
        }
#endif
    }

    public bool Opened(Player player) {
        return openedFor.Contains(player);
    }

    public void OpenFor(Player player, bool open = true) {
        Debug.Log(string.Format("Door {0} open for {1} : {2}", this, player, open));
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), open);
        if (open) {
            openedFor.Add(player);
        } else {
            openedFor.Remove(player);
        }
    }

    void Update() {
#if UNITY_EDITOR
        if (Extensions.Editor()) {
            if (keyColor != oldKeyColor) {
                oldKeyColor = keyColor;
                closed = opened = keyColor.color;
                opened.a = openedAlpha;
                closed.a = closedAlpha;
            }
            colored.color = closed;
        } else {
#endif
            GetComponent<MeshRenderer>().material.ChangeAlpha(Opened(Player.current) ? openedAlpha : closedAlpha);
#if UNITY_EDITOR
        }
#endif
    }
}
