/*
    Class de gestion des barres de vie des ennemis
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 15/04/2025;
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    [field: SerializeField] GameObject cam;
    public event Action<EnemyHpBar> OnTargetDeath;
    private Transform target;
    [field: SerializeField] private RectTransform rect;
    [field: SerializeField] private Image fillImage;
    
    public void Init(GameObject target)
    {   
        cam = Camera.main.gameObject;
        this.target = target.transform;
        fillImage.fillAmount = 1;
        target.GetComponent<EnnemiMain>().OnDammageTaken += HandleOnDammageTaken;
        target.GetComponent<EnnemiMain>().OnEnemyDeath += HandleOnEnemyDeath;
    }

    private void FixedUpdate()
    {
        rect.position = target.position + (Vector3.up * 1.9f);
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }

    private void HandleOnDammageTaken(float newHp, float totalHp)
    {
        fillImage.fillAmount = newHp / totalHp;
    }

    private void HandleOnEnemyDeath(EnnemiMain instance)
    {
        target.GetComponent<EnnemiMain>().OnDammageTaken -= HandleOnDammageTaken;
        target.GetComponent<EnnemiMain>().OnEnemyDeath -= HandleOnEnemyDeath;
        OnTargetDeath?.Invoke(this);
    }
}