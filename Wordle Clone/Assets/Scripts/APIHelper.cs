using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class APIHelper : MonoBehaviour
{
    [Serializable]
    public struct Word
    {
        public string word;
    }

    public Word word;

    private void Awake()
    {
        StartCoroutine(GetWordCo());
    }


    public Word GetWord()
    {
        StartCoroutine(GetWordCo());
        return word;
    }

    IEnumerator GetWordCo()
    {
        string url = "https://asia-south1-dj-ui-dev.cloudfunctions.net/wordlewordV2";
        using(UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if(request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("error");
            }

            else
            {
                word = JsonUtility.FromJson<Word>(request.downloadHandler.text);
            }
        }
    }
}
    