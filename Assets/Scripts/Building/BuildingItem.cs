﻿using System;
using UnityEngine;

[Serializable]
public class BuildingItem {
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public Block Prefab
    {
        get { return _prefab; }
        set { _prefab = value; }
    }

    [SerializeField]
    private string _title;
    [SerializeField]
    private Block _prefab;
}
