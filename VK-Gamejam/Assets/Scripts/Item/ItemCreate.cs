using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Item;

namespace EasyButtons
{
    public class ItemCreate : MonoBehaviour
    {
        [SerializeField] private GameObject _item;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _itemSprite;
        [SerializeField] private Sprite _itemFlipped;
        [SerializeField] private IsRotate _rotation;



        [Button]
        public void SaveAsPrefab()
        {
            CreateItem();
            string localPath = "Assets/Prefabs/Objects/" + _name + ".prefab";

            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            PrefabUtility.SaveAsPrefabAssetAndConnect(_item, localPath, InteractionMode.UserAction);
        }

        private void CreateItem()
        {
            Item item = _item.AddComponent<Item>();
            item.Initialize(_name, _itemSprite, _itemFlipped, _rotation);

            if (_item.TryGetComponent(out SpriteRenderer renderer) != true)
            {
                SpriteRenderer spriteRender = _item.AddComponent<SpriteRenderer>();
                spriteRender.sprite = _itemSprite;

            }
        }

    }
}

