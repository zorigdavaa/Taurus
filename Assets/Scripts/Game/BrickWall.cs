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
                crackRenderer.gameObject.SetActive(false);
                GetComponent<Collider>().enabled = false;
                foreach (var item in parts)
                {
                    item.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
    }
}