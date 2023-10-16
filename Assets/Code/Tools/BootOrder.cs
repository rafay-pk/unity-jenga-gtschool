using System.Collections.Generic;
using UnityEngine;

namespace Code.Tools
{
	// Has higher priority in execution order from Project Settings  
	public class BootOrder :  MonoBehaviour
	{
		[SerializeField] private List<SingletonBase> singletons;
		private void Awake()
		{
			singletons.ForEach(singleton =>
			{
				singleton.Initialize();
				// print($"{singleton.GetType()} -- Initialized");
			}); 
		}
	}
}
