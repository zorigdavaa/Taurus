using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;
using ZPackage.Helper;
using Random = UnityEngine.Random;
using UnityEngine.Pool;

public class Player : Character
{
    [SerializeField] List<GameObject> SkillPrefabs;
    CameraController cameraController;
    SoundManager soundManager;
    UIBar bar;
    URPPP effect;
    int killCount;
    int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        soundManager = FindObjectOfType<SoundManager>();
        cameraController = FindObjectOfType<CameraController>();
        // line.positionCount = LineResolution;
        effect = FindObjectOfType<URPPP>();
        // bar = FindObjectOfType<UIBar>();
        // bar.gameObject.SetActive(false);
        GameManager.Instance.GameOverEvent += OnGameOver;
        GameManager.Instance.GamePlay += OnGamePlay;
        GameManager.Instance.LevelCompleted += OnGameOver;
        // cameraController.Zoom(0.5f, 20, () => cameraController.Zoom(1, 60));
    }

    private void Skill0(object sender, EventArgs e)
    {
        GameObject InstantiatedFireBall = Instantiate(SkillPrefabs[0], transform.position + Vector3.up * 2, Quaternion.identity);
        Physics.IgnoreCollision(GetComponent<Collider>(), InstantiatedFireBall.GetComponentInChildren<Collider>());
        InstantiatedFireBall.transform.rotation = transform.rotation;

        // Physics.CheckCapsule(transform.position, transform.position + transform.forward * 5, 2, 1 << 3);//only 3 layer
    }

    internal void IncreaseKillCount()
    {
        killCount++;
        if (killCount >= enemyCount)
        {
            GameManager.Instance.LevelComplete(this, 0);
        }

    }

    private void Skill1(object sender, EventArgs e)
    {
        GameObject InstantiatedFireBall = Instantiate(SkillPrefabs[1], transform.position + Vector3.up * 3, Quaternion.identity, transform);
        // Physics.IgnoreCollision(GetComponent<Collider>(), InstantiatedFireBall.GetComponentInChildren<Collider>());
        InstantiatedFireBall.transform.rotation = transform.rotation;

    }
    private void Skill2(object sender, EventArgs e)
    {
        GameObject InstantiatedFireBall = Instantiate(SkillPrefabs[2], transform.position + transform.forward * 10, Quaternion.identity);
        // Physics.IgnoreCollision(GetComponent<Collider>(), InstantiatedFireBall.GetComponentInChildren<Collider>());
        InstantiatedFireBall.transform.rotation = transform.rotation;
    }

    private void Update()
    {
        if (IsAlive)
        {
            FindNearestEnemy();
        }
    }
    [SerializeField] Transform Target = null;

    private void FindNearestEnemy()
    {
        float shortest = 100;
        Transform nearest = null;
        foreach (var item in Physics.OverlapSphere(transform.position, 10, 1 << 3))
        {
            float Distance = Vector3.Distance(transform.position, item.transform.position);
            if (shortest > Distance)
            {
                nearest = item.transform;
                shortest = Distance;
            }
        }

        Target = nearest;
        if (Target)
        {
            animationController.Throw();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collect collect = other.GetComponent<Collect>();
        if (collect)
        {
            inventory.AddInventory(collect.gameObject);
        }
    }
    public override void Die()
    {
        base.Die();
        GameManager.Instance.GameOver(this, EventArgs.Empty);
    }

    private void OnGamePlay(object sender, EventArgs e)
    {
        movement.SetSpeed(1);
        movement.SetControlAble(true);
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        // throw new NotImplementedException();
    }
}
