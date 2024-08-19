using System;
using NaughtyAttributes;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [Tag] [SerializeField] public string objectsTag;
	[Tag] [SerializeField] public string npcTag;
	[SerializeField] private int mouseButton;
	[SerializeField] private Grab otherHand;
    [SerializeField] private bool isHolding;

	public Rigidbody2D _GrabbedObject 
	{
		get 
		{
			if (GetComponent<FixedJoint2D>() != null) 
			{
				return GetComponent<FixedJoint2D>().connectedBody;
			} else return null;
		}
	}

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

    private void OnTriggerEnter2D(Collider2D other)
	{
		Transform otherTransform = other.transform;
		GameObject otherRoot = otherTransform.root.gameObject;
		Rigidbody2D otherRb = otherTransform.GetComponent<Rigidbody2D>();

		if (!isHolding || !(otherRoot.CompareTag(objectsTag) || otherRoot.CompareTag(npcTag))) return;
		if (otherHand._GrabbedObject != null && otherHand._GrabbedObject != otherRb) return;
		
		FixedJoint2D fixedJoint2D = gameObject.AddComponent<FixedJoint2D>();
		
		if (fixedJoint2D == null) return;
		fixedJoint2D.connectedBody = otherRb;
	}
}
