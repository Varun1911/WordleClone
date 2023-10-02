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
}
