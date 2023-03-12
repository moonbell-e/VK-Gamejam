using FMODUnity;
using Grid;
using InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objects;
    [SerializeField] private EventReference _openBox;
    [SerializeField] private EventReference _closeBox;

    private ParticleSystem _puff;
    public List<GameObject> Objects => _objects;

    private float _timeUpdateAlpha = 0.02f;

    private void Start()
    {
        _puff = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (InputHandler.Instance.ObjectInHand != null) 
        {
            this.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.GetComponent<Button>().interactable = true;
        }
    }

    public void TakeAnItem()
    {
        if (_objects.Count > 0)
        {
            RuntimeManager.PlayOneShot(_openBox);
            GameObject obj = GetRandomObject();
            PlaceableObject spawnObj = SpawnObject(obj);
            InputHandler.Instance.TakeObjectInHand(spawnObj);
            _objects.Remove(obj);
        }
        else
        {
            StartCoroutine(BoxDisappearing(gameObject.GetComponent<Image>()));
        }
    }

    private PlaceableObject SpawnObject(GameObject obj)
    {
        GameObject spawnObj = Instantiate(obj);
        SpriteRenderer renderer = spawnObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartCoroutine(AnimationInstantiation(renderer));
        return spawnObj.GetComponent<PlaceableObject>(); ;
    }

    private GameObject GetRandomObject()
    {
        return _objects[Random.Range(0, _objects.Count)];
    }

    private IEnumerator AnimationInstantiation(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        float alpha = 0f;

        while (spriteRenderer.color.a >= 0)
        {
            alpha += 0.05f;

            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);

            yield return new WaitForSeconds(_timeUpdateAlpha);
        }
    }


    private IEnumerator BoxDisappearing(Image image)
    {
        image.color = new Color(1f, 1f, 1f, 1f);
        float alpha = 1f;

        while (image.color.a >= 0)
        {
            alpha -= 0.05f;

            image.color = new Color(1f, 1f, 1f, alpha);

            yield return new WaitForSeconds(_timeUpdateAlpha);
        }

        _puff.Play();
        RuntimeManager.PlayOneShot(_closeBox);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
