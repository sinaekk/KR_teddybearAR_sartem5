
/*
 * 작성자: Kim, Bummoo
 * 작성일: 2024.12.04
 */
using FUTUREVISION.WebCamera;
using FUTUREVISION.WebAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FUTUREVISION
{
    public enum EGlobalState
    {
        None,

        // 테스트 모드
        FilterMode,
        WorldMode,
    }

    public class GlobalManager : BaseManager<GlobalManager>
    {
        [Header("GlobalManager")]
        public DataModel DataModel;
        public NetworkModel NetworkModel;
        public SoundModel SoundModel;

        private EGlobalState currentGlobalState = EGlobalState.None;

        public EGlobalState CurrentGlobalState
        {
            get { return currentGlobalState; }
        }

        protected override void Awake()
        {
            base.Awake();

            Initialize();

            WebARManager.Instance.Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            // 모델 초기화
            DataModel.Initialize();
            NetworkModel.Initialize();
            SoundModel.Initialize();
        }
    }
}