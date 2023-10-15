using System.Runtime.InteropServices;
using UnityEngine;

namespace Plugins.Hyperlink
{
	public class OpenURL : MonoBehaviour 
	{
		[DllImport("__Internal")]
		public static extern void openWindow(string url);
	}
}