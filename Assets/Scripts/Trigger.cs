using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Reflection;

public class Trigger : MonoBehaviour
{
	[Tag] [SerializeField] private string filterTag;
	[SerializeField] private string otherObjectFunctionToExecute;
	[SerializeField] private MonoBehaviour otherType;
	private Type _otherType;
	
	private void Start()
	{
		_otherType = otherType.GetType();
	}
	
    public UnityEvent enterTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
		
		if (_otherType != null && !string.IsNullOrEmpty(otherObjectFunctionToExecute)) 
		{
			Type scriptType = Type.GetType(_otherType.Name);
			if (scriptType != null) 
			{
				Debug.Log("type: " + scriptType);
				Component scriptComponent = other.transform.GetComponent(scriptType);
				if (scriptComponent != null) 
				{
					Debug.Log("component: " + scriptComponent);
					MethodInfo method = scriptType.GetMethod(otherObjectFunctionToExecute);
					if (method != null) 
					{
						Debug.Log("method: " + method.Name);
						method.Invoke(scriptComponent, null);
					}
				}
			}
		}
		if (string.IsNullOrEmpty(filterTag)) 
		{
			enterTrigger?.Invoke();
		} else if (other.CompareTag(filterTag))
        {
            enterTrigger?.Invoke();
        }
    }
}
