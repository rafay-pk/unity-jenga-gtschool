using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Code
{
    public class StackCreator : MonoBehaviour
    {
        [SerializeField] private DataProvider dataProvider;
        [SerializeField] private GameObject blockPrefab, glassPrefab, woodPrefab, stonePrefab, textPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float stackOffset;
        private Vector3 verticalOffset, positionResetOffset, xStep, zStep; 
        private void Awake()
        {
            dataProvider ??= GetComponent<DataProvider>();
            dataProvider.OnDataFetchComplete.AddListener(CreateStacks);
            
            var blockDimensions = blockPrefab.transform.localScale;
            verticalOffset = new Vector3(0f, blockDimensions.y / 3, 0f);
            positionResetOffset = 2f * new Vector3(blockDimensions.x, 0f, -blockDimensions.x);
            xStep = new Vector3(blockDimensions.x, 0f, 0f);
            zStep = new Vector3(0f, 0f, blockDimensions.x);
        }
        private void CreateStacks()
        {
            var stacks = dataProvider.Data.GroupBy(dp => dp.grade);
            var stackPosition = spawnPoint.position;
            foreach (var stack in stacks)
            {
                var grade = stack.Key;
                var orderedData = stack
                    .OrderBy(dp => dp.domain)
                    .ThenBy(dp => dp.cluster)
                    .ThenBy(dp => dp.standardID).ToList();

                var stackParent = new GameObject(grade.ToString());
                Instantiate(textPrefab, stackParent.transform).GetComponentInChildren<TMP_Text>().text = stackParent.name;

                var position = stackPosition;
                var rotated = false;

                for (var i = 0; i < orderedData.Count; i++)
                {
                    if (i % 3 == 0 && i != 0)
                    {
                        rotated = !rotated;
                        position = stackPosition + i * verticalOffset;
                        if (rotated) position += positionResetOffset;
                    }
                    var prefab = orderedData[i].mastery switch
                    {
                        Mastery.NeedToLearn => glassPrefab,
                        Mastery.Learned => woodPrefab,
                        Mastery.Mastered => stonePrefab,
                        Mastery.Null => throw new ArgumentOutOfRangeException(),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    position += rotated ? zStep : xStep;
                    var rotation = rotated ? Quaternion.Euler(0f, 90f, 0f) : Quaternion.identity;
                    Instantiate(prefab, position, rotation, stackParent.transform);
                }

                stackPosition += Vector3.right * stackOffset;
            }
        }
    }
}
