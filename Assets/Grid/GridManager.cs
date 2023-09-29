using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{


    [SerializeField] private int _width, _height;
    [SerializeField] private int x1, x2, y1, y2;
    [SerializeField] private int x21, x22, y21, y22;
    [SerializeField] private int x31, x32, y31, y32;
    [SerializeField] private Tile _tileprefab;
    [SerializeField] private Tile _tileprefab2;
    [SerializeField] private Tile _tileprefab3;
    [SerializeField] private Tile _tileprefab4;
    [SerializeField] private Transform _cam;
    void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        //Green
        for (int x = -1 * _width / 2; x < _width / 2; x++)
        {
            for (int y = -1 * _height / 2; y < _height / 2; y++)
            {
                var spawnedTile = Instantiate(_tileprefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile ({x}, {y})";
                var isOdd = ((x + y) % 2 == 1 || (x + y) % 2 == -1);
                Debug.Log($"Tile ({x}, {y}) isOdd: " + $"{(x + y) % 2}");
                spawnedTile.Init(isOdd);
            }

        }

        //Yellow
        for (int x = x1; x < x2; x++)
        {
            for (int y = y1; y < y2; y++)
            {
                var spawnedTile = Instantiate(_tileprefab2, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile ({x}, {y})";
                var isOdd = ((x + y) % 2 == 1 || (x + y) % 2 == -1);
                Debug.Log($"Tile ({x}, {y}) isOdd: " + $"{(x + y) % 2}");
                spawnedTile.Init(isOdd);
            }

        }

        //Blue
        for (int x = x21; x < x22; x++)
        {
            for (int y = y21; y < y22; y++)
            {
                var spawnedTile = Instantiate(_tileprefab3, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile ({x}, {y})";
                var isOdd = ((x + y) % 2 == 1 || (x + y) % 2 == -1);
                Debug.Log($"Tile ({x}, {y}) isOdd: " + $"{(x + y) % 2}");
                spawnedTile.Init(isOdd);
            }

        }

        //Grey
        for (int x = x31; x < x32; x++)
        {
            for (int y = y31; y < y32; y++)
            {
                var spawnedTile = Instantiate(_tileprefab4, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile ({x}, {y})";
                var isOdd = ((x + y) % 2 == 1 || (x + y) % 2 == -1);
                Debug.Log($"Tile ({x}, {y}) isOdd: " + $"{(x + y) % 2}");
                spawnedTile.Init(isOdd);
            }

        }

    }

}
