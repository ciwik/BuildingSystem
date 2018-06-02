using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UiBuildingItem : MonoBehaviour
{
    private Text _text;
    private Button _button;
    private Image _image;

    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private Color _unselectedColor;

    internal void Awake()
    {
        _text = GetComponentInChildren<Text>();
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();        
    }

    internal void Start()
    {
        Unselect();
    }

    public void Init(string text, Action callback)
    {
        _text.text = text;
        _button.onClick.AddListener(() => callback());        
    }

    public void Select()
    {
        _image.color = _selectedColor;
    }

    public void Unselect()
    {
        _image.color = _unselectedColor;
    }
}
