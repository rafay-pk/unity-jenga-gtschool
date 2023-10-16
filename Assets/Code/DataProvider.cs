using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Tools;
using Code.UserInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Code
{
    public class DataProvider : Singleton<DataProvider>
    {
        [Header("Asset References")] 
        [SerializeField] private TextAsset jsonFile;
        [Header("Data")] 
        [SerializeField] private bool fetchDataFromURL;
        [SerializeField] private string apiUrl = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
        private readonly List<StandardDataPoint> data = new();
        public IEnumerable<StandardDataPoint> Data => data;
        [Header("Events")]
        public UnityEvent OnDataFetchComplete;
        
        #region Unity Functions
        private void Awake()
        {
            if (fetchDataFromURL)
                StartCoroutine(FetchData());
            else ProcessData(jsonFile.text);
        }
        #endregion
        
        #region Private Functions
        private IEnumerator FetchData()
        {
            using var www = UnityWebRequest.Get(apiUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                #if UNITY_EDITOR
                Debug.LogError("Error: " + www.error);
                #endif
                NotificationPanel.Instance.ShowNotification(www.error);
            }
            else if (www.isDone) ProcessData(www.downloadHandler.text);
        }
        private void ProcessData(string text)
        {
            text = "{ \"dataPoints\":" + text + "}";
            var dataList = JsonUtility.FromJson<StandardDataListParser>(text);
            data.AddRange(dataList.dataPoints.Select(obj => new StandardDataPoint(obj)));
            OnDataFetchComplete.Invoke();
        }
        #endregion
    }
}
