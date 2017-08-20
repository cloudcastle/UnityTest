using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SearchManager : MonoBehaviour
{
    public static SearchManager instance;
    public List<PortalSurface> portalSurfaces;

    void Awake() {
        instance = this;
        UpdateSearchData();
    }

    public void UpdateSearchData() {
        portalSurfaces = FindObjectsOfType<PortalSurface>().ToList();
    }
}