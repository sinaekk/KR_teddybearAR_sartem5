/*
 * 작성자: Kim Bummoo
 * 작성일: 2025.03.03
 *
 */

using Imagine.WebAR;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace FUTUREVISION.WebAR
{
    public enum EARTrackerState
    {
        None,
        ScreenState,
        WorldState,
    }

    public class ARTrackerModel : BaseModel
    {
        [Header("AR Camera의 포지션과 이름이 하드코딩 되어있으니 변경하지 말것")]

        [Space(10)]
        [Header("Camera")]
        [SerializeField] protected ARCamera ARCamera;
        [SerializeField] protected ScreenshotManager ScreenshotManager;
        [SerializeField] protected GameObject Placement;

        [Space(10)]
        [SerializeField] protected Camera ScreenContentCamera;
        [SerializeField] protected Camera WorldContentsCamera;

        public Camera GetScreenContentCamera()
        {
            return ScreenContentCamera;
        }

        public Camera GetWorldContentsCamera()
        {
            return WorldContentsCamera;
        }

        [Space(10)]
        [Header("Object")]
        [SerializeField] protected WorldTracker WorldTracker;

        [Space(10)]
        [SerializeField] protected GameObject ScreenController;
        [SerializeField] protected GameObject WorldController;

        [Space(10)]
        [SerializeField] protected GameObject ScreenObject;
        [SerializeField] protected GameObject WorldObject;

        [Space(10)]
        [Header("State")]
        [SerializeField] protected EARTrackerState ARTrackingState;

        [Space(10)]
        [Header("Event")]
        public UnityEvent<EARTrackerState> OnARTrackingStateChanged;

        public UnityEvent OnPlaced;
        public UnityEvent OnReset;
        public UnityEvent<bool> OnPlacementVisibilityChanged;
        public UnityEvent<EScreenShotEventType> OnScreenShotEvent;

        public override void Initialize()
        {
            base.Initialize();

        }

        protected void SetARTrackerState(EARTrackerState state)
        {
            ARTrackingState = state;

            switch (ARTrackingState)
            {
                case EARTrackerState.ScreenState:
                    WorldTracker.ResetOrigin();

                    // 카메라 전환
                    ScreenContentCamera.gameObject.SetActive(true);
                    WorldContentsCamera.gameObject.SetActive(false);

                    // 
                    ScreenController.SetActive(true);
                    WorldController.SetActive(false);

                    // 오브젝트 전환
                    ScreenObject.SetActive(true);
                    WorldObject.SetActive(false);
                    break;

                case EARTrackerState.WorldState:
                    WorldTracker.ResetOrigin();

                    // 카메라 전환
                    ScreenContentCamera.gameObject.SetActive(false);
                    WorldContentsCamera.gameObject.SetActive(true);

                    //
                    ScreenController.SetActive(false);
                    WorldController.SetActive(true);

                    // 오브젝트 전환
                    ScreenObject.SetActive(false);
                    // 플레이스 한 후에 활성화
                    //WorldObject.SetActive(true);
                    WorldObject.SetActive(false);
                    break;
            }

            OnARTrackingStateChanged?.Invoke(ARTrackingState);
        }

        public EARTrackerState GetARTrackerState()
        {
            return ARTrackingState;
        }

        public GameObject GetCurruentObject()
        {
            switch (ARTrackingState)
            {
                case EARTrackerState.ScreenState:
                    return ScreenObject;
                case EARTrackerState.WorldState:
                    return WorldObject;
            }

            return null;
        }

        public bool GetPlacementVisibility()
        {
            return Placement.activeSelf;
        }

        public void SetScreenState()
        {
            SetARTrackerState(EARTrackerState.ScreenState);
        }

        public void ResetPlacement()
        {
            SetARTrackerState(EARTrackerState.WorldState);
            OnReset?.Invoke();
            
            WorldTracker.PlaceOrigin();
        }

        public void FlipCamera()
        {
            ARCamera.FlipCamera();
        }

        public void TakeScreenShot()
        {
            // 스크린샷 매니저를 통해 스크린샷을 찍습니다. WebGL에서 정상작동 합니다.
            ScreenshotManager.GetScreenShot();
        }


        // WorldTracker 이벤트 콜백

        public void OnPlacementVisibilityChangedCallback(bool isShow)
        {
            OnPlacementVisibilityChanged?.Invoke(isShow);
        }
    }
}
