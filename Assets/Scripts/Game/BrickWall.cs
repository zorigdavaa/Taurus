using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : MonoBehaviour
{
    [SerializeField] List<Transform> parts;
    [SerializeField] MeshRenderer crackRenderer;
    float crackPercent = 0;
    bool called = false;
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            crackPercent += 0.005f;
            crackRenderer.material.SetFloat("_CrackSize", crackPercent);
            if (crackPercent > 0.4f && !called)
            {
                called = true;
                other.gameObject.GetComponent<Movement>().SetSpeed(1f);
                CrackAll();
            }
        }
    }

    private void CrackAll()
    {
        Crack();
        Collider[] nearObjs = Physics.OverlapBox(transform.position, new Vector3(10, 1, 1), Quaternion.identity);
        foreach (var item in nearObjs)
        {
            BrickWall wall = item.GetComponent<BrickWall>();
            if (wall)
            {
                wall.Crack();
            }
        }
    }
    public void Crack()
    {
        crackRenderer.gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;

        foreach (var item in parts)
        {
            item.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Movement>().SetSpeed(0.5f);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Movement>().SetSpeed(1f);
        }
    }
}
