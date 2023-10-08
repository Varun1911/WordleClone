using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] tiles {  get; private set; }

    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
    }

    public void SubmitRowAnimation()
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            LeanTween.scaleY(tiles[i].gameObject, 0, 0.2f).setDelay(0.2f * i);
            LeanTween.scaleY(tiles[i].gameObject, 1, 0.2f).setDelay(0.2f * (i+1));
        }
    }


    public void NotEnoughLettersAnimation()
    {
        int shakeCount = 3;
        float shakeTime = 0.075f;

        for (int i = shakeCount; i > 0;i--)
        {
            LeanTween.moveLocalX(gameObject, -10f, shakeTime).setDelay(2 * shakeTime * i);
            LeanTween.moveLocalX(gameObject, 10f, shakeTime).setDelay(shakeTime + 2 * shakeTime * i);
        }

        LeanTween.moveLocalX(gameObject, 0f, shakeTime).setDelay(2 * shakeTime * (shakeCount + 1));
    }
}
