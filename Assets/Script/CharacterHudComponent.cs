using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHudComponent : MonoBehaviour
{

    private static GameObject hpBarResource;
    private GameObject hpBar;
    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");

        if (hpBarResource == null)
        {
            hpBarResource = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefab/HP.prefab", typeof(GameObject)) as GameObject;
        }
        hpBar = UnityEngine.Object.Instantiate(hpBarResource, canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
}
