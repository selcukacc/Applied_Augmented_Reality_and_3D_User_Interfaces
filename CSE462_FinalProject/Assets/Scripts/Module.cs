using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Module : MonoBehaviour
{
    public string nameOfPrefab;
    private ModuleNavigator moduleNavigator;

    void Start()
    {
        //nameOfPrefab = GetComponent<MeshFilter>().mesh.name.Replace(" Instance", "");
        transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = nameOfPrefab;
        moduleNavigator = FindObjectOfType<ModuleNavigator>();
    }

    public void Click()
    {
        moduleNavigator.receivedName = nameOfPrefab;
        moduleNavigator.Spawn();
    }


    public void University()
    {
        nameOfPrefab = "university_mobile";
        moduleNavigator.receivedName = nameOfPrefab;
        moduleNavigator.Spawn();
    }

    public void House()
    {
        nameOfPrefab = "house_2_v_21_mobile";
        moduleNavigator.receivedName = nameOfPrefab;
        moduleNavigator.Spawn();
    }

    public void MetalStore()
    {
        nameOfPrefab = "metal_store";
        moduleNavigator.receivedName = nameOfPrefab;
        moduleNavigator.Spawn();
    }

    public void Publisher()
    {
        nameOfPrefab = "publisher_mobile";
        moduleNavigator.receivedName = nameOfPrefab;
        moduleNavigator.Spawn();
    }
}
