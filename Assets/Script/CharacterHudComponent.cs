using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHudComponent : MonoBehaviour
{

    private static GameObject hpBarResource;
    private GameObject hpBar;
    private GameObject canvas;

    // Start is called before the first frame update
    void Awake()
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
        hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 40, 0);
    }

    public void InitializeHp(int current, int max)
    {
        var slider = hpBar.GetComponent<Slider>();
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = max;
            slider.value = current;
        }
    }

    public void UpdateHp(int current)
    {
        var slider = hpBar.GetComponent<Slider>();
        if (slider != null)
        {
            slider.value = current;
        }
    }
}
