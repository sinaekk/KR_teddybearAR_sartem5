/*
 * 작성자: Kim Bummoo
 * 작성일: 2025.03.03
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FUTUREVISION.WebAR
{
    public class ARObjectView : BaseView
    {
        [Header("Object")]
        public GameObject ParentObject;
        public List<GameObject> ObjectList = new List<GameObject>();
        protected int CurrentObjectIndex = 0;

        public override void Initialize()
        {
            // 초기화
            SetCurrentObject(0);
        }

        protected virtual void Update()
        {
            var targetObject = WebARManager.Instance.ARTrackerModel.GetCurruentObject();

            ParentObject.transform.position = targetObject.transform.position;
            ParentObject.transform.rotation = targetObject.transform.rotation;
            ParentObject.transform.localScale = targetObject.transform.localScale;
            ParentObject.gameObject.SetActive(targetObject.activeSelf);
        }   

        public void SetCurrentObject(int index)
        {
            // 범위를 벗어나면 경고 후 종료
            if (index < 0 || index >= ObjectList.Count)
            {
                Debug.LogWarning("Index out of range");
                return;
            }

            CurrentObjectIndex = index;

            for (int i = 0; i < ObjectList.Count; i++)
            {
                ObjectList[i].SetActive(i == index);
            }
        }

        public int GetCurrentObjectIndex()
        {
            return CurrentObjectIndex;
        }
    }
}
