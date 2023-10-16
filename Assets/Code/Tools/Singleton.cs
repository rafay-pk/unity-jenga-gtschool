using UnityEngine;

namespace Code.Tools
{
	public abstract class SingletonBase : MonoBehaviour
	{
		public abstract void Initialize();
	}
	public class Singleton <T> : SingletonBase where T : MonoBehaviour
	{
		public static T Instance { get; protected set; }
        
		public override void Initialize()
		{
			if (Instance == null)
				SetInstance(this as T);
			else Destroy(this);
		}
        
		protected virtual void SetInstance(T instance) => Instance = instance;
	}
	public class SingletonDontDestroy <T> : Singleton<T> where T : MonoBehaviour
	{
		protected override void SetInstance(T instance)
		{
			Instance = instance;
			DontDestroyOnLoad(instance);
		}
	}
}