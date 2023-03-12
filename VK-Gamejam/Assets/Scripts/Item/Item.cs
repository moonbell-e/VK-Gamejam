using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _item;
    [SerializeField] private Sprite _itemFlipped;
    [SerializeField] private IsRotate _rotation;

    public enum IsRotate
    {
        None,
        Rotate
    }

    public void Initialize(string name, Sprite item, Sprite itemFlipped, IsRotate rotation)
    {
        _name = name;
        _item = item;
        _itemFlipped = itemFlipped;
        _rotation = rotation;
    }
}
