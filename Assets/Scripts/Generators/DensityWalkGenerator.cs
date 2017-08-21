using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using System;
using System.Threading;


public class DensityWalkGenerator : MonoBehaviour
{
    public GameObject planeSample;
    public GameObject lakeSample;
    public GameObject bonusSample;
    public GameObject boxSample;
    public GameObject portalSample;

    public UnityEvent onGenerated;

    public GameObject world;
    public List<GameObject> walls;


    float[,] densities;
    float[,] baseDensities;
    int[,] map;

    bool Inside(int[,] map, int x, int y) {
        return 0 <= x && x < map.GetLength(0) && 0 <= y && y < map.GetLength(1);
    }

    int Get(int[,] map, int x, int y) {
        if (!Inside(map, x, y)) {
            return -322;
        }
        return map[x, y];
    }

    bool Taken(int[,] map, int x, int y) {
        return Get(map, x, y) == 1;
    }

    List<IntVector2> Neighbours8(int[,] map, int x, int y, Func<int, bool> free) {
        List<IntVector2> result = new List<IntVector2>();
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i != 0 || j != 0) {
                    if (free(Get(map, x+i, y+j))) {
                        result.Add(new IntVector2(x+i, y+j));
                    }
                }
            }
        }
        return result;
    }

    List<IntVector2> Neighbours(int[,] map, int x, int y, Func<int, bool> free) {
        List<IntVector2> result = new List<IntVector2>();
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i != 0 || j != 0) {
                }
            }
        }
        if (free(Get(map, x + 1, y))) {
            result.Add(new IntVector2(x + 1, y));
        }
        if (free(Get(map, x - 1, y))) {
            result.Add(new IntVector2(x - 1, y));
        }
        if (free(Get(map, x, y + 1))) {
            result.Add(new IntVector2(x, y + 1));
        }
        if (free(Get(map, x, y - 1))) {
            result.Add(new IntVector2(x, y - 1));
        }

        return result;
    }

    void Start() {
        Generate();
        onGenerated.Invoke();
        SearchManager.instance.UpdateSearchData();
        FindObjectsOfType<PortalSurface>().ForEach(ps => ps.alwaysRecursiveRender = true);
    }

    void AddIfNot(int[,] map, IntVector2 p, List<IntVector2> list) {
        if (Get(map, p.x, p.y) == 0) {
            list.Add(p);
            map[p.x, p.y] = 2;
        }
    }

    void AddNeighbours(int[,] map, IntVector2 next, List<IntVector2> list) {
        AddIfNot(map, new IntVector2(next.x - 1, next.y), list);
        AddIfNot(map, new IntVector2(next.x + 1, next.y), list);
        AddIfNot(map, new IntVector2(next.x, next.y - 1), list);
        AddIfNot(map, new IntVector2(next.x, next.y + 1), list);
    }

    public void ReduceDensity(int x, int y, float delta) {
        if (!Inside(map, x, y)) {
            return;
        }
        densities[x, y] -= delta;
        if (densities[x, y] < 0) {
            densities[x, y] = 0;
        }
    }

    private void Generate() {
        world = new GameObject("World");

        int n = 500;
        int steps = 6000;
        int bonuses = 30;
        int boxes = 1000;
        int portals = 0;


        densities = new float[n, n];
        baseDensities = new float[n, n];

        map = new int[n, n];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                //float distance = Mathf.Sqrt(Mathf.Pow(i - n / 2, 2) + Mathf.Pow(j - n / 2, 2));
                //densities[i, j] = 1 + Mathf.Pow(distance, distance / 50);
                //densities[i, j] = 1 + Mathf.Pow(distance, distance / 50);
                baseDensities[i,j] = densities[i,j];

                //densities[i, j] = Mathf.Pow(UnityEngine.Random.Range(0, 1f), -6);
                //densities[i, j] = 1-Mathf.Pow(UnityEngine.Random.Range(0, 1f), 24);
                densities[i, j] = Mathf.Pow(2, UnityEngine.Random.Range(0, 1f)*60);
                if (densities[i, j] <= 0) {
                    Debug.LogFormat("ALARM");
                }
            }
        }

        IntVector2 start = new IntVector2(n/2, n/2);
        List<IntVector2> list = new List<IntVector2>();
        List<IntVector2> secondary = new List<IntVector2>();
        List<IntVector2> cells = new List<IntVector2>();
        list.Add(start);

        for (int t = 0; t < steps; t++) {
            IntVector2 next = list.Rnd(cell => densities[cell.x, cell.y]);
            //IntVector2 next = list.MinBy(cell => densities[cell.x, cell.y]);
            if (UnityEngine.Random.Range(0, 1) < 1f) {
                map[next.x, next.y] = 1;
                float densityReduction = baseDensities[next.x, next.y] / 3;
                //ReduceDensity(next.x - 1, next.y, densityReduction);
                //ReduceDensity(next.x + 1, next.y, densityReduction);
                //ReduceDensity(next.x, next.y - 1, densityReduction);
                //ReduceDensity(next.x, next.y + 1, densityReduction);
            }
            list.Remove(next);
            AddNeighbours(map, next, list);
        }
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (map[i, j] == 1) {
                    map[i, j] = 1;
                } else {
                    map[i, j] = 0;
                }
            }
        }
        HashSet<IntVector2> reached = new HashSet<IntVector2>();
        Thread thread = new Thread(() => {
            Algorithms.Dfs(reached, new IntVector2(0, 0), cell => {
                return Neighbours8(map, cell.x, cell.y, x => x == 0);
            });
        }, 1024000000);
        thread.Start();
        thread.Join();
        Debug.LogFormat("Reached: {0}", reached.Count);

        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (map[i, j] == 1) {
                    Build(world, n, i, j);
                    cells.Add(new IntVector2(i,j));
                    //if (Get(map, i - 1, j) == 0) {
                    //    BuildLeftWall(world, n, i, j);
                    //}
                    //if (Get(map, i, j - 1) == 0) {
                    //    BuildTopWall(world, n, i, j);
                    //}
                    //if (Get(map, i + 1, j) == 0) {
                    //    BuildLeftWall(world, n, i + 1, j, true);
                    //}
                    //if (Get(map, i, j + 1) == 0) {
                    //    BuildTopWall(world, n, i, j + 1, true);
                    //}
                }
                if (map[i, j] == 0 && !reached.Contains(new IntVector2(i, j))) {
                    BuildLake(world, n, i, j);
                }
            }
        }

        //for (int i = 0; i < bonuses; i++) {
        //    BuildBonus(world, cells.Rnd(), n);
        //}
        //for (int i = 0; i < boxes; i++) {
        //    BuildBox(world, cells.Rnd(), n);
        //}
        //for (int i = 0; i < portals; i++) {
        //    BuildPortalPair();
        //}
    }

    Portal BuildPortal() {
        var wall = walls.rnd();
        walls.Remove(wall);
        var portal = Instantiate(portalSample, wall.transform.position, Quaternion.Euler(wall.transform.localEulerAngles.Change(y: wall.transform.localEulerAngles.y)), world.transform);
        Destroy(wall);
        return portal.GetComponent<Portal>();
    }

    private void BuildPortalPair() {
        var p1 = BuildPortal();
        var p2 = BuildPortal();
        p1.Connect(p2);
    }

    private void BuildBox(GameObject world, IntVector2 p, int n) {
        var box = Instantiate(boxSample, new Vector3((p.x - n / 2) * 10 + UnityEngine.Random.Range(-4.5f, 4.5f), 1f, (p.y - n / 2) * 10 + UnityEngine.Random.Range(-4.5f, 4.5f)), Quaternion.Euler(0, UnityEngine.Random.Range(0, 90), 0), world.transform);
        box.transform.localScale = new Vector3(2, 2, 2);
        if (UnityEngine.Random.Range(0, 1f) < 0.5) {
            box.transform.localScale = new Vector3(6, 6, 6);
            box.transform.position = box.transform.position.Change(y: 3);
        }
        if (UnityEngine.Random.Range(0, 1f) < 0.5) {
            box.transform.localScale = new Vector3(4, 4, 4);
            box.transform.position = box.transform.position.Change(y: 2);
        }
    }

    private void BuildBonus(GameObject world, IntVector2 p, int n) {
        Instantiate(bonusSample, new Vector3((p.x - n / 2) * 10 + UnityEngine.Random.Range(-4.5f, 4.5f), UnityEngine.Random.Range(1, 7f), (p.y - n / 2) * 10 + UnityEngine.Random.Range(-4.5f, 4.5f)), Quaternion.identity, world.transform);
    }

    private void Build(GameObject world, int n, int i, int j) {
        var ceil = Instantiate(planeSample, new Vector3((i - n / 2) * 10, 10, (j - n / 2) * 10), Quaternion.Euler(90, 0, 0), world.transform);
        ceil.transform.localScale = Vector3.one;
        //if (UnityEngine.Random.Range(0, 1f) < 0.1f) {
        //    return;
        //}
        var plane = Instantiate(planeSample, new Vector3((i - n / 2) * 10, 0, (j - n / 2) * 10), Quaternion.Euler(-90, 0, 0), world.transform);
        plane.transform.localScale = Vector3.one;
    }

    private void BuildLake(GameObject world, int n, int i, int j) {
        var plane = Instantiate(lakeSample, new Vector3((i - n / 2) * 10, 0, (j - n / 2) * 10), Quaternion.Euler(-90, 0, 0), world.transform);
        plane.transform.localScale = Vector3.one;
    }

    private void BuildLeftWall(GameObject world, int n, int i, int j, bool reversed = false) {
        var plane = Instantiate(planeSample, new Vector3((i - n / 2) * 10 - 5, 5, (j - n / 2) * 10), Quaternion.Euler(0,reversed ? -90 : 90 ,0), world.transform);
        plane.transform.localScale = Vector3.one;
        walls.Add(plane);
    }

    private void BuildTopWall(GameObject world, int n, int i, int j, bool reversed = false) {
        var plane = Instantiate(planeSample, new Vector3((i - n / 2) * 10, 5, (j - n / 2) * 10 - 5), Quaternion.Euler(0, reversed ? 180 : 0, 0), world.transform);
        plane.transform.localScale = Vector3.one;
        walls.Add(plane);
    }
}