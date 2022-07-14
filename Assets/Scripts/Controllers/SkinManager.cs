using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> skins;
    [SerializeField] private GameObject skinPrefab;
    [SerializeField] private GameObject skinHolder;
    private int _selectedSkin;
    private List<SkinPrefab> _skinPrefabs = new();
    public static SkinManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        for (int i = 0; i < skins.Count; i++)
        {
            SkinPrefab skinPrefabComponent = Instantiate(skinPrefab, skinHolder.transform).GetComponent<SkinPrefab>();
            skinPrefabComponent.SetSkin(skins[i]);
            skinPrefabComponent.SetIndex(i);
            _skinPrefabs.Add(skinPrefabComponent);
        }
        _selectedSkin = 0;
        _skinPrefabs[_selectedSkin].SetBorder(true);
    }

    public void SelectSkin(int index)
    {
        if (_selectedSkin == index) return;
        _selectedSkin = index;
        for (int i = 0; i < _skinPrefabs.Count; i++)
        {
            _skinPrefabs[i].SetBorder(i == index);
        }
    }

    public Sprite GetSkin()
    {
        return skins[_selectedSkin];
    }
}