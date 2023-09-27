using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{


    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tileprefab;
    [SerializeField] private Transform _cam;
    void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
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

    }

}
