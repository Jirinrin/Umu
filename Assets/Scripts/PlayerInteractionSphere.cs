using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerInteractionSphere : MonoBehaviour
{
    private CustomPlatformer2DUserControl _player;
    [SerializeField] private SpriteRenderer interactionHint;

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<CustomPlatformer2DUserControl>();
        interactionHint.color = C.ColorAlpha0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<Npc>();
        _player.Interactable = interactable;
        interactable.FocusEnter();
        interactionHint.DOColor(C.ColorAlpha1, .5f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _player.Interactable = null;
        other.GetComponent<Npc>().FocusExit();
        interactionHint.DOColor(C.ColorAlpha0, .5f);
    }
}
