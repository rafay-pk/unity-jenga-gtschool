using System;
using System.Collections.Generic;
using System.Linq;
using Code.Tools;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public class StackCreator : Singleton<StackCreator>
    {
        [Header("Asset References")] 
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private GameObject glassPrefab, woodPrefab, stonePrefab;
        [Header("Component References")]
        [SerializeField] private Transform spawnPoint;
        [Header("Data")]
        [SerializeField] private float stackOffset;
        [SerializeField] private Vector3 positionResetOffset = new (2f, 0f, -2f);
        [SerializeField] private Vector3 verticalOffset = new (0f, 0.2f, 0f);
        [SerializeField] private Vector3 xStep = new (1f, 0f, 0f);
        [SerializeField] private Vector3 zStep = new (0f, 0f, 1f);
        private readonly Dictionary<Grade, List<Block>> stackDictionary = new();
        public IEnumerable<Block> GetBlocks(Grade grade) => stackDictionary[grade];

        #region Unity Functions
        private void Awake()
        {
            DOTween.SetTweensCapacity(500, 125);
        }
        private void OnEnable()
        {
            DataProvider.Instance.OnDataFetchComplete.AddListener(CreateStacks);
        }
        private void OnDisable()
        {
            DataProvider.Instance.OnDataFetchComplete.RemoveListener(CreateStacks);
        }
        #endregion
        
        #region Private Functions
        private void CreateStacks()
        {
            var stacks = DataProvider.Instance.Data.GroupBy(dp => dp.grade);
            var stackPosition = spawnPoint.position;
            foreach (var stack in stacks)
            {
                var grade = stack.Key;
                stackDictionary[grade] = new List<Block>();
                var orderedData = stack
                    .OrderBy(dp => dp.domain)
                    .ThenBy(dp => dp.cluster)
                    .ThenBy(dp => dp.standardID).ToList();

                var stackParent = new GameObject(StandardDataPoint.GradeString(grade));
                stackParent.transform.position = stackPosition;
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
                    var rotation = rotated ? Quaternion.Euler(0f, 90f, 0f) : Quaternion.Euler(0f, 180f, 0f);
                    var go = Instantiate(prefab, position, rotation, stackParent.transform);
                    var block = go.GetComponentInChildren<Block>();
                    block.standardDataPoint = orderedData[i];
                    stackDictionary[grade].Add(block);
                    go.transform.DOLocalJump(go.transform.localPosition, Random.Range(1f, 15f), 1, 1f)
                        .SetEase(Ease.OutExpo);
                }

                stackPosition += Vector3.right * stackOffset;
            }
        }
        #endregion
    }
}
