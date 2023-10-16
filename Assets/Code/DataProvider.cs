using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Code
{
    public class DataProvider : Singleton<DataProvider>
    {
        [Header("Data")]
        [SerializeField] private string apiUrl = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
        private readonly List<StandardDataPoint> data = new();
        public IEnumerable<StandardDataPoint> Data => data;
        [Header("Events")]
        public UnityEvent OnDataFetchComplete;
        private void Awake()
        {
            StartCoroutine(FetchData());
        }
        private IEnumerator FetchData()
        {
            using var www = UnityWebRequest.Get(apiUrl);
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
