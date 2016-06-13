using UnityEngine;
using System.Collections;

public class BlinkAbility : Ability
{
    RaycastHit hit;

    public Transform eye;
    public Transform legs;
    public GameObject phantom;

    const float MAX_DIST = 1000f;
    const float BASE_DIST = 15f;

    int Mask() {
        return ~LayerMask.GetMask("Ghost");
    }

    void ShowPhantom(Vector3 point) {
        phantom.SetActive(true);
        phantom.transform.position = point;
    }

    void HidePhantom() {
        phantom.SetActive(false);
    }

    bool AcceptablePoint(Vector3 point) {
        SpaceScanner.count = Physics.CapsuleCastNonAlloc(point + Vector3.up / 2, point - Vector3.up / 2 + Vector3.up * 0.1f, 0.5f, Vector3.forward, SpaceScanner.rayCastResults, 0, Mask());
        return SpaceScanner.count == 0;
    }

    bool CheckAirPoint() {
        bool b = Portal.Raycast(new Ray(eye.position, eye.forward), out hit, mask: Mask());
        if (b && hit.distance < BASE_DIST) {
            return false;
        } 
        var airPoint = eye.position + eye.forward * BASE_DIST;
        if (AcceptablePoint(airPoint)) {
            ShowPhantom(airPoint);
            return true;
        }
        return false;
    }


    bool CheckEyePoint() {
        bool b = Portal.Raycast(new Ray(eye.position, eye.forward), out hit, mask: Mask());
        if (!b) {
            return false;
        }
        if (hit.normal.y < 0.01f) {
            return false;
        }
        var eyePoint = hit.point + hit.normal.normalized / 2 + Vector3.up / 2;
        if (AcceptablePoint(eyePoint)) {
            ShowPhantom(eyePoint);
            return true;
        }
        return false;
    }

    bool CheckLegsPoint() {
        SpaceScanner.count = Physics.RaycastNonAlloc(new Ray(legs.position + eye.forward * MAX_DIST, -eye.forward), SpaceScanner.rayCastResults, MAX_DIST, Mask());
        if (SpaceScanner.count < 2) {
            return false;
        }
        for (int i = SpaceScanner.count - 1; i >= 0; i--) {
            RaycastHit hit = SpaceScanner.rayCastResults[i];
            if (hit.collider.gameObject == gameObject) {
                continue;
            }
            if (hit.normal.y < 0.01f) {
                continue;
            }
            var dist = MAX_DIST - hit.distance;
            RaycastHit eyeHit;
            bool b = Portal.Raycast(new Ray(eye.position, eye.forward), out eyeHit);
            if (b && eyeHit.distance < dist) {
                continue;
            }
            var legPoint = hit.point + hit.normal.normalized / 2 + Vector3.up / 2;
            if (!AcceptablePoint(legPoint)) {
                continue;
            }
            ShowPhantom(legPoint);
            return true;
        }
        return false;
    }

    void Update() {
        if (TimeManager.instance.Undoing()) {
            return;
        }
        if (Player.instance.Blink()) {
            if (phantom.activeSelf) {
                transform.position = phantom.transform.position;
                GetComponent<Move>().velocity = Vector3.zero;
                HidePhantom();
            }
        }
        if (!Player.instance.BlinkPrepare()) {
            HidePhantom();
            return;
        }
        if (CheckEyePoint()) {
            return;
        }
        if (CheckLegsPoint()) {
            return;
        }
        if (CheckAirPoint()) {
            return;
        }
        HidePhantom();
    }
}