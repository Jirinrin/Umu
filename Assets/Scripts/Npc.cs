using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

// todo: make generic Interactable class which this inherits some stuff from
// [RequireComponent(typeof(CircleCollider2D))]
public class Npc : MonoBehaviour
{
    [SerializeField] private List<string> lines;

    [SerializeField] private Vector3 textBoxPosition = new Vector3(0, 3f, 0);
    
    private List<string>.Enumerator _line;

    private Collider2D _collider;

    private SpriteRenderer _textBox;
    private TextMeshPro _textBoxText;

    private void FadeIn()
    {
        _textBox.DOColor(C.ColorAlpha1, .5f);
        _textBoxText.DOColor(Color.black, .5f);
    }

    private void FadeOut()
    {
        _textBox.DOColor(C.ColorAlpha0, .5f);
        _textBoxText.DOColor(Color.clear, .5f);
    }

    private void OnEnable()
    {
        _line = lines.GetEnumerator();

        _collider = GetComponentInChildren<Collider2D>();
        if (!_collider)
            _collider = gameObject.AddComponent<CircleCollider2D>();
        _collider.isTrigger = true;

        var textBoxPrefab = Resources.Load("Prefabs/TextBox") as GameObject;
        
        var textBox = Instantiate(textBoxPrefab, transform);
        textBox.transform.localPosition = textBoxPosition;
        _textBox = textBox.GetComponent<SpriteRenderer>();
        _textBoxText = textBox.GetComponentInChildren<TextMeshPro>();
        _textBox.color = C.ColorAlpha0;
        _textBoxText.text = null;
    }

    public void Confirm()
    {
        if (!_line.MoveNext())
            _line = lines.GetEnumerator();
        
        FadeIn();

        DisplayLine(_line.Current);
    }

    public void FocusEnter()
    {
        if (_line.Current != null)
            FadeIn();
    }
    
    public void FocusExit()
    {
        FadeOut();
    }

    private void DisplayLine(string line)
    {
        Debug.Log("display line:");
        Debug.Log(line);
        _textBoxText.text = line;
        if (line == null)
            FadeOut();
    }
}
