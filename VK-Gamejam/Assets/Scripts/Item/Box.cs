using Grid;
using InputSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objects;
    public List<GameObject> Objects => _objects;

    private float _timeUpdateAlpha = 0.02f;

    private void OnMouseDown()
    {
        if (_objects.Count > 0)
        {
            GameObject obj = GetRandomObject();
            PlaceableObject spawnObj = SpawnObject(obj);
            InputHandler.Instance.SetPlaceableObject(spawnObj);
            _objects.Remove(obj);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private PlaceableObject SpawnObject(GameObject obj)
    {
        GameObject spawnObj = Instantiate(obj);
        spawnObj.transform.position = gameObject.transform.position;
        StartCoroutine(AnimateInstantiation(spawnObj));
        return spawnObj.GetComponent<PlaceableObject>(); ;
    }

    private GameObject GetRandomObject()
    {
        return _objects[Random.Range(0, _objects.Count)];
    }

    private IEnumerator AnimateInstantiation(GameObject obj)
    {
        SpriteRenderer spriteRenderer= obj.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        float alpha = 0f;

        while (spriteRenderer.color.a >= 0)
        {
            alpha += 0.05f;

            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);

            yield return new WaitForSeconds(_timeUpdateAlpha);
        }

    }
}
