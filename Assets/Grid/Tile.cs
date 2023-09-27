using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _primaryColor, _secondaryColor;
    [SerializeField] private SpriteRenderer _renderer;

    public void Init(bool isOdd)
    {
        _renderer.color = isOdd ? _primaryColor : _secondaryColor;
    }
}
