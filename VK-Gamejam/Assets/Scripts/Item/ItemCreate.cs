using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyButtons
{
    public class ItemCreate : MonoBehaviour
    {
        [SerializeField] private GameObject _item;
        [SerializeField] private GameObject _placeableObject;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _itemSprite;
        [SerializeField] private Sprite _itemFlipped;
        [SerializeField] private IsRotate _rotation;


#if UNITY_EDITOR
        [Button]
        public void SaveAsPrefab()
        {
            CreateItem();
            string localPath = "Assets/Prefabs/Objects/" + _name + ".prefab";

            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            PrefabUtility.SaveAsPrefabAssetAndConnect(_placeableObject, localPath, InteractionMode.UserAction);
        }
#endif

        private void CreateItem()
        {
            Item item = _item.AddComponent<Item>();
            _placeableObject.AddComponent<PlaceableObject>();

            item.transform.SetParent(_placeableObject.transform);
            item.Initialize(_name, _itemSprite, _itemFlipped, _rotation);

            if (_item.TryGetComponent(out SpriteRenderer renderer) != true)
            {
                SpriteRenderer spriteRender = _item.AddComponent<SpriteRenderer>();
                spriteRender.sprite = _itemSprite;
            }
        }

    }
}

