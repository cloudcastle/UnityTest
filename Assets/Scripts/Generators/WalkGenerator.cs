using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WalkGenerator : MonoBehaviour
{
    public GameObject planeSample;

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

    int Neighbours(int[,] map, int x, int y) {
        int result = 0;
        if (Taken(map, x + 1, y)) {
            result++;
        }
        if (Taken(map, x - 1, y)) {
            result++;
        }
        if (Taken(map, x, y + 1)) {
            result++;
        }
        if (Taken(map, x, y - 1)) {
            result++;
        }
        return result;
    }

    void Start() {
        Generate();
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

    private void Generate() {
        var world = new GameObject("World");

        int n = 1000;
        int steps = 5000;
        int[,] map = new int[n, n];
        IntVector2 start = new IntVector2(n/2, n/2);
        List<IntVector2> list = new List<IntVector2>();
        list.Add(start);

        for (int t = 0; t < steps; t++) {
            if (list.Count == 0) {
                break;
            }
            IntVector2 next = list.Rnd();
            map[next.x, next.y] = 1;
            list.Remove(next);
            AddNeighbours(map, next, list);
            while (list.Count > 30 && Random.Range(0, 1) < Mathf.Pow(0.4f, list.Count - 10)) {
                list.RemoveAt(Random.Range(0, list.Count));
            }
        }
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (map[i, j] == 1 || map[i, j] == 2) {
                    map[i, j] = 1;
                } else {
                    map[i, j] = 0;
                }
            }
        }
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (map[i, j] == 1) {
                    Build(world, n, i, j);
                    if (Get(map, i - 1, j) == 0) {
                        BuildLeftWall(world, n, i, j);
                    }
                    if (Get(map, i, j - 1) == 0) {
                        BuildTopWall(world, n, i, j);
                    }
                    if (Get(map, i + 1, j) == 0) {
                        BuildLeftWall(world, n, i + 1, j);
                    }
                    if (Get(map, i, j + 1) == 0) {
                        BuildTopWall(world, n, i, j + 1);
                    }
                }
            }
        }
    }

    private void Build(GameObject world, int n, int i, int j) {
        var plane = Instantiate(planeSample, new Vector3((i - n / 2) * 10, 0, (j - n / 2) * 10), Quaternion.identity, world.transform);
        plane.transform.localScale = Vector3.one;
    }

    private void BuildLeftWall(GameObject world, int n, int i, int j) {
        var plane = Instantiate(planeSample, new Vector3((i - n / 2) * 10 - 5, 5, (j - n / 2) * 10), Quaternion.Euler(0,0,90), world.transform);
        plane.transform.localScale = Vector3.one;
    }

    private void BuildTopWall(GameObject world, int n, int i, int j) {
        var plane = Instantiate(planeSample, new Vector3((i - n / 2) * 10, 5, (j - n / 2) * 10 - 5), Quaternion.Euler(0, 90, 90), world.transform);
        plane.transform.localScale = Vector3.one;
    }
}