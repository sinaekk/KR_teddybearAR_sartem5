using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using FUTUREVISION.WebAR;


namespace Imagine.WebAR
{
    public enum EScreenShotEventType
    {
        None,
        Prepare,
        Capture,
        Release,
    }

    public class ScreenshotManager : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void ShowWebGLScreenshot(string dataUrl);
#endif
        public ARTrackerModel ARTrackerModel;
        private ARCamera arCamera;

        [SerializeField] private AudioClip shutterSound;
        [SerializeField] private AudioSource shutterSoundSource;

        public Texture2D screenShot;


        void Start(){
            arCamera = GameObject.FindObjectOfType<ARCamera>();
        }


        public void GetScreenShot()
        {
            StartCoroutine(CaptureScreenshot());
        }

        private IEnumerator CaptureScreenshot()
        {
            yield return new WaitForEndOfFrame();

            ARTrackerModel.OnScreenShotEvent.Invoke(EScreenShotEventType.Prepare);

            // 현재 프레임이 모두 그려질 때까지 기다립니다.
            yield return new WaitForEndOfFrame();

            ARTrackerModel.OnScreenShotEvent.Invoke(EScreenShotEventType.Capture);

            // 기존 텍스처 삭제하여 메모리 누수를 방지합니다.
            if (screenShot != null)
            {
                Destroy(screenShot);
            }
            // 스크린 사이즈에 맞춰 Texture2D 생성
            screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);

            // 화면 버퍼의 픽셀을 읽어옵니다.
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenShot.Apply();

            //// 테스트를 위해 사진 저장
            //string path = Application.dataPath + "/screenshot.png";
            //System.IO.File.WriteAllBytes(path, screenShot.EncodeToPNG());

            // 이후 스크린샷 저장 등 후속 처리
            ARTrackerModel.OnScreenShotEvent.Invoke(EScreenShotEventType.Release);

            if (shutterSoundSource != null && shutterSound != null)
            {
                shutterSoundSource.PlayOneShot(shutterSound);
            }

#if UNITY_EDITOR
            Debug.Log("Screenshots are only displayed on WebGL builds");
#elif UNITY_WEBGL && !UNITY_EDITOR
    byte[] textureBytes = screenShot.EncodeToJPG();
    string dataUrlStr = "data:image/jpeg;base64," + System.Convert.ToBase64String(textureBytes);
    ShowWebGLScreenshot(dataUrlStr);
#endif
        }
    }
}

