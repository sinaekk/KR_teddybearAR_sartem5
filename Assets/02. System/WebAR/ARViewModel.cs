/*
 * 작성자: Kim Bummoo
 * 작성일: 2025.03.03
 *
 */

using Imagine.WebAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FUTUREVISION.WebAR
{

    public class ARViewModel : BaseViewModel
    {
        [Space(10)]
        [SerializeField] protected ARObjectView ARObjectView;
        [SerializeField] protected BackgroundView BackgroundView;
        [SerializeField] protected ARUIView ARUIView;

        public override void Initialize()
        {
            ARObjectView.Initialize();
            BackgroundView.Initialize();
            ARUIView.Initialize();

            // Callback Bindings
            WebARManager.Instance.ARTrackerModel.OnScreenShotEvent.AddListener((eventType) =>
            {
                switch (eventType)
                {
                    case EScreenShotEventType.Prepare:
                        {
                            ARUIView.gameObject.SetActive(false);
                        }
                        break;
                    case EScreenShotEventType.Release:
                        {
                            ARUIView.gameObject.SetActive(true);
                        }
                        break;
                }
            });

            GlobalManager.Instance.DataModel.OnDataLoaded.AddListener((data) =>
            {
                // 데이터 로드 후 처리
                BackgroundView.Message.Text.text = data;
            });
            BackgroundView.Message.Text.text = GlobalManager.Instance.DataModel.Message;

            //WebARManager.Instance.ARTrackerModel.OnARTrackingStateChanged.AddListener((state) =>
            //{
            //    OnARTrackingStateChanged(state);
            //});
            WebARManager.Instance.ARTrackerModel.OnPlacementVisibilityChanged.AddListener((isVisible) =>
            {
                ARUIView.SetPlaceButtonInteractable(isVisible);
            });

            WebARManager.Instance.ARTrackerModel.ResetPlacement();
            SetObejct(0);

            // Event Bindings
            ARUIView.SwitchCameraButton.Button.onClick.AddListener(() => {
                // 전면 카메라, 후면 카메라 전환
                WebARManager.Instance.ARTrackerModel.FlipCamera();

                // AR 모드를 전환합니다.
                SwitchSetCurrentObjectState();
            });
            ARUIView.PlaceButton.Button.onClick.AddListener(() => {
                // AR 오브젝트를 배치합니다.
                WebARManager.Instance.ARTrackerModel.ResetPlacement();
            });
            ARUIView.TakeScreenshotButton.Button.onClick.AddListener(() => {
                // 스크린샷을 찍습니다.
                WebARManager.Instance.ARTrackerModel.TakeScreenShot();
            });
            //ARUIView.SwitchARButton.Button.onClick.AddListener(() =>
            //{
            //    // AR 모드를 전환합니다.
            //    SwitchSetCurrentObjectState();
            //});

            ARUIView.BeforeButton.Button.onClick.AddListener(() => { SetObejct(ARObjectView.GetCurrentObjectIndex() - 1); });
            ARUIView.NextButton.Button.onClick.AddListener(() => { SetObejct(ARObjectView.GetCurrentObjectIndex() + 1); });
        }

        public void SwitchSetCurrentObjectState()
        {
            var currentObjectState = WebARManager.Instance.ARTrackerModel.GetARTrackerState();
            switch (currentObjectState)
            {
                case EARTrackerState.ScreenState:
                    {
                        // 바로 WorldState로 전환
                        WebARManager.Instance.ARTrackerModel.ResetPlacement();
                    }
                    break;
                case EARTrackerState.WorldState:
                    {
                        WebARManager.Instance.ARTrackerModel.SetScreenState();
                    }
                    break;
            }
        }

        //public void OnARTrackingStateChanged(EARTrackerState arVeiwState)
        //{
        //    // AR 상태에 따른 UI 변경
        //    switch (arVeiwState)
        //    {
        //        case EARTrackerState.ScreenState:
        //            //ARUIView.SetActivePlacedButton(false);
        //            BackgroundView.SetActiveBacgkround(true);
        //            break;
        //        case EARTrackerState.WorldState:
        //            //ARUIView.SetActivePlacedButton(false);
        //            BackgroundView.SetActiveBacgkround(false);
        //            break;
        //    }
        //}

        public void SetObejct(int newIndex)
        {
            //// 순환하지 않을 때 처리
            //ARObjectView.SetCurrentObject(newIndex);

            //bool canBefore = newIndex > 0;
            //bool canNext = newIndex < ARObjectView.ObjectList.Count - 1;
            //ARUIView.SetActiveStepButton(canBefore, canNext);

            // 순환하도록 처리
            newIndex = (newIndex + ARObjectView.ObjectList.Count) % ARObjectView.ObjectList.Count;
            ARObjectView.SetCurrentObject(newIndex);

            //ARUIView.SetActiveStepButton(true, true);
        }
    }
}
