using System;
using NaughtyAttributes;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [Tag] [SerializeField] public string objectsTag;
	[Tag] [SerializeField] public string npcTag;
    [SerializeField] private bool isHolding;
    [SerializeField] private int mouseButton;

    private void Update()
    {
        if (Input.GetMouseButton(mouseButton))
        {
            isHolding = true;
        }
        else
        {
            isHolding = false;
            Destroy(GetComponent<FixedJoint2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
		Transform otherTransform = other.transform;
        if (isHolding && (otherTransform.root.gameObject.CompareTag(objectsTag) || otherTransform.root.gameObject.CompareTag(npcTag)))
        {
            Rigidbody2D otherRb = otherTransform.GetComponent<Rigidbody2D>();
            FixedJoint2D fixedJoint2D = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
            if (otherRb != null && fixedJoint2D != null)
            {
                fixedJoint2D.connectedBody = otherRb;
            }
        }
    }
}
