using System.Net;
using System.IO;
using UnityEngine;

public class APIHelper : MonoBehaviour
{ 
    public static Word GetNewWord()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://asia-south1-dj-ui-dev.cloudfunctions.net/wordlewordV2");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<Word>(json);
    }

}
