using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace FUTUREVISION
{
    public class DataModel : BaseModel
    {
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
        public string ID = "테스트 ID";
        public string Message = "";

        // 공개된 CSV URL (예: 구글 시트의 경우 시트ID와 export 파라미터 포함)
        public string csvUrl = "https://docs.google.com/spreadsheets/d/1ykRlU_1313KEArLXg5AXMIZA6d8U5u_NfhBzQAIsV60/export?format=csv";

        public UnityEvent<string> OnDataLoaded = new UnityEvent<string>();

        public override void Initialize()
        {
            // URL 파라미터에서 값 가져오기
            var url = Application.absoluteURL;

            if (url.Contains("?"))
            {
                var param = url.Split('?')[1];  // ? 뒤의 파라미터 부분만 가져오기
                var paramList = param.Split('&');
                Parameters = new Dictionary<string, string>();

                foreach (var p in paramList)
                {
                    var keyValue = p.Split('=');
                    if (keyValue.Length == 2)
                    {
                        // URL 디코딩 적용 (특수문자 및 한글 처리)
                        string key = WWW.UnEscapeURL(keyValue[0]);
                        string value = WWW.UnEscapeURL(keyValue[1]);
                        Parameters[key] = value;
                    }
                }
            }

            // "id" 키가 있는 경우 디코딩된 값 사용
            ID = Parameters.ContainsKey("id") ? Parameters["id"] : ID;

            // CSV 데이터 로드 함수 호출
            StartCoroutine(LoadCSVData());
        }

        IEnumerator LoadCSVData()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(csvUrl))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || 
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("CSV 로드 에러: " + request.error);
                }
                else
                {
                    string csvText = request.downloadHandler.text;
                    ProcessCSV(csvText);
                }
            }
        }

        // CSV 데이터를 간단히 파싱하여 로그에 출력하는 예제
        void ProcessCSV(string csvText)
        {
            // CSV의 줄바꿈을 모두 '\n' 기준으로 통일
            csvText = csvText.Replace("\r\n", "\n").Replace("\r", "\n");

            // '"'로 시작할 경우 '"'까지 읽어들이고, 아닐 경우 ','까지 csv 값을 읽어들입니다.
            // 읽어들인 후, 줄바꿈인 경우 다음 줄로 이동합니다. 줄바꿈을 먼저 하는 경우 ""로 감싸진 값의 줄바꿈 처리가 복잡해집니다.
            List<List<string>> values = new List<List<string>>();

            string value = "";
            bool inQuote = false;
            List<string> currentLine = new List<string>();

            foreach (char c in csvText)
            {
                if (c == '"')
                {
                    inQuote = !inQuote;
                }
                else if (c == ',' && !inQuote)
                {
                    currentLine.Add(value);
                    value = "";
                }
                else if (c == '\n' && !inQuote)
                {
                    currentLine.Add(value);
                    values.Add(currentLine);
                    currentLine = new List<string>();
                    value = "";
                }
                else
                {
                    value += c;
                }
            }
            // 마지막 줄 처리
            currentLine.Add(value);
            values.Add(currentLine);

            // ID를 찾아서 콜백 호출
            foreach (var line in values)
            {
                if (line.Count > 0 && line[0] == ID)
                {
                    Message = line.Count > 1 ? line[1] : "";
                    break;
                }
            }
            OnDataLoaded?.Invoke(Message);
        }
    }
}
