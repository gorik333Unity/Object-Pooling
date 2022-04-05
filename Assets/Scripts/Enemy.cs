using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private const float COLOR_CHANGE_DURATION = 0.3f;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private Color _colorOnDamage;

    [SerializeField]
    private float _health = 10f;

    private Color _normalColor;
    private Coroutine _colorChangeC;

    public void TakeDamage(float damage)
    {
        if (damage < 0)
            damage = 0;

        _health -= damage;

        ColorChange();
        CheckDeath();
    }

    private void Start()
    {
        _normalColor = _meshRenderer.material.color;
    }

    private void ColorChange()
    {
        if (_colorChangeC != null)
            StopCoroutine(_colorChangeC);

        _colorChangeC = StartCoroutine(ColorChangeOnTakeDamage());
    }

    private bool CheckDeath()
    {
        if (_health <= 0)
        {
            Debug.Log("Died");

            return true;
        }

        return false;
    }

    private IEnumerator ColorChangeOnTakeDamage()
    {
        var effectTime = 0f;
        var multiplier = 1 / COLOR_CHANGE_DURATION;
        _meshRenderer.material.color = _colorOnDamage;

        while (true)
        {
            effectTime += Time.deltaTime * multiplier;

            var color = Color.Lerp(_colorOnDamage, _normalColor, effectTime);
            _meshRenderer.material.color = color;

            if (effectTime >= COLOR_CHANGE_DURATION * multiplier)
                yield break;

            yield return null;
        }
    }
}
