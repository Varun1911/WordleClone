using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [Serializable]
    public class State
    {
        public Color fillColor; 
        public Color outlineColor;
    }

    public char letter { get; private set; }
    public State state { get; private set; }


    private TextMeshProUGUI textMeshPro;
    private Image tileImage;
    private Outline tileOutline;

    private void Awake()
    {
        textMeshPro  = GetComponentInChildren<TextMeshProUGUI>();
        tileImage = GetComponent<Image>();
        tileOutline = GetComponent<Outline>();
    }


    public void SetLetter(char letter)
    {
        textMeshPro.text = letter.ToString().ToUpper();
        this.letter = letter;
    }

    
    public void SetState(State state)
    {
        this.state = state;
        tileImage.color = state.fillColor;
        tileOutline.effectColor = state.outlineColor;
    }
}
