using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Code
{
    public class DataProvider : MonoBehaviour
    {
        [SerializeField] private string API_URL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
        [SerializeField] private List<StandardDataPoint> data;
        public IEnumerable<StandardDataPoint> Data => data;
        public UnityEvent OnDataFetchComplete;
        private void Awake()
        {
            data = new List<StandardDataPoint>();
            StartCoroutine(FetchData());
        }
        private IEnumerator FetchData()
        {
            using var www = UnityWebRequest.Get(API_URL);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else if (www.isDone)
            {
                var jsonResult = www.downloadHandler.text;
                jsonResult = "{ \"dataPoints\":" + jsonResult + "}";
                var dataList = JsonUtility.FromJson<StandardDataListParser>(jsonResult);
                data.AddRange(dataList.dataPoints.Select(obj => new StandardDataPoint(obj)));
                OnDataFetchComplete.Invoke();
            }
        }
    }
}
