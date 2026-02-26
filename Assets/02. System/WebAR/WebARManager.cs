/*
 * 작성자: Kim Bummoo
 * 작성일: 2024.12.11
 */

using Imagine.WebAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FUTUREVISION.WebAR
{
    public class WebARManager : BaseManager<WebARManager>
    {
        public ARTrackerModel ARTrackerModel;
        public ARViewModel ARViewModel;
        public GameObject IntroObject;

        [Space(10)]
        public RenderTexture RenderTexture;

        public override void Initialize()
        {
            base.Initialize();

            ARTrackerModel.Initialize();
            ARViewModel.Initialize();

            IntroObject.SetActive(true);
            StartCoroutine(RequestCameraPermission(() =>
            {
                // 카메라 권한 요청 후 초기화
            }));
        }
        

        public IEnumerator RequestCameraPermission(Action action)
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);

            // 카메라 권한 요청

            if (Application.isEditor)
            {
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                yield return new WaitUntil(() =>
                {
                    return Application.HasUserAuthorization(UserAuthorization.WebCam);
                });
            }

            yield return new WaitForSeconds(1.5f);

            // 카메라 권한 요청 후 카메라 초기화
            ARTrackerModel.gameObject.SetActive(true);
            action?.Invoke();

            yield return new WaitForSeconds(1.5f);

            // if (IsAutomaticPlacement)
            {
                ARTrackerModel.ResetPlacement();
                IntroObject.SetActive(false);
            }
        }

        public void Update()
        {
            // Render Texture의 크기를 스크린 크기에 맞춰 업데이트
            if (RenderTexture.width != Screen.width || RenderTexture.height != Screen.height)
            {
                RenderTexture.Release();
                RenderTexture.width = Screen.width;
                RenderTexture.height = Screen.height;
                RenderTexture.Create();
            }
        }
    }
}
