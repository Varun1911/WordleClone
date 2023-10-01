using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    //array of all possible letter inputs
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, 
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z           
    };

    private Row[] rows;

    //current row and column indices
    private int rowInd;
    private int colInd;

    //the answer
    [SerializeField]private string correctWord;

    [SerializeField] private ParticleSystem confetti1, confetti2;
    [SerializeField] private APIHelper apiHelper;

    [Header("States")]
    [SerializeField] private Tile.State emptyState;
    [SerializeField] private Tile.State filledState;
    [SerializeField] private Tile.State correctState;
    [SerializeField] private Tile.State wrongSpotState;
    [SerializeField] private Tile.State incorrectState;

    [Header("UI")]
    [SerializeField] private Button newWordButton;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private GameObject answerText;
    [SerializeField] private GameObject hintButton;


    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }


    private void Start()
    {
        SetCorrectWord();
    }


    private void OnEnable()
    {
        //hide buttons when game starts
        newWordButton.gameObject.SetActive(false);
        tryAgainButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        //show buttons when game ends
        newWordButton.gameObject.SetActive(true);
        tryAgainButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        Row currRow = rows[rowInd];
        
        //backspace pressed
        if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            colInd = Mathf.Max(colInd - 1, 0);
            currRow.tiles[colInd].SetLetter('\0');
            currRow.tiles[colInd].SetState(emptyState);
        }

        //row already filled
        else if (colInd >= currRow.tiles.Length)
        {
            //submit current guess
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SubmitRow(currRow);
            }
        }

        //getting letters
        else
        {
            for(int i =0; i < SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                {
                    currRow.tiles[colInd].SetLetter((char)SUPPORTED_KEYS[i]);
                    currRow.tiles[colInd].SetState(filledState);
                    colInd++;
                    break;
                }
            }
        }

    }


    private void SubmitRow(Row currRow)
    {
        //we need 2 for loops cause we need to handle the edge case where our guess has
        //a letter 2 times or more but the correct word has it only once (or less number of times than the guess)
        //example correctWord is belts and you guess bells

        string alteredWord = correctWord;

        for (int i = 0; i < currRow.tiles.Length; i++)
        {
            Tile currTile = currRow.tiles[i];

            if (currTile.letter == correctWord[i])
            {
                //correct state
                currTile.SetState(correctState);
                alteredWord = alteredWord.Remove(i, 1);
                alteredWord = alteredWord.Insert(i, " ");
            }

            else if(!correctWord.Contains(currTile.letter))
            {
                //incorrect state
                currTile.SetState(incorrectState);
            }
        }


        for (int i = 0; i < currRow.tiles.Length; i++)
        {
            Tile currTile = currRow.tiles[i];

            if(currTile.state != correctState && currTile.state != incorrectState)
            {
                //wrong spot
                if(alteredWord.Contains(currTile.letter))
                {
                    currTile.SetState(wrongSpotState);

                    int letterCorrectIndex = alteredWord.IndexOf(currTile.letter);
                    alteredWord = alteredWord.Remove(letterCorrectIndex, 1);
                    alteredWord = alteredWord.Insert(letterCorrectIndex, " ");
                }

                else
                {
                    currTile.SetState(incorrectState);
                }
            }
        }


        //check if the player guessed the correct word
        if(HasWon(currRow))
        {
            enabled = false;
            confetti1.Play();
            confetti2.Play();
        }

        rowInd++;
        colInd = 0;

        if(rowInd >= rows.Length)
        {
            //gameover
            enabled = false;
        }

    }


    private void ClearBoard()
    {
        rowInd = 0; 
        colInd = 0;

        for(int i=0; i<rows.Length; i++)
        {
            for(int j=0; j < rows[i].tiles.Length; j++)
            {
                rows[i].tiles[j].SetLetter('\0');
                rows[i].tiles[j].SetState(emptyState);
            }
        }
    }

    private void SetCorrectWord()
    {
        correctWord = apiHelper.GetWord().word;
        correctWord = correctWord.Trim();
        answerText.GetComponentInChildren<TextMeshProUGUI>().text = correctWord.Trim().ToUpper();
    }


    private bool HasWon(Row currRow)
    {
        for(int i=0; i<currRow.tiles.Length; i++)
        {
            if (currRow.tiles[i].state != correctState)
            {
                return false;
            }
        }

        return true;
    }

    public void NewGame()
    {
        SetCorrectWord();
        ClearBoard();
        enabled = true;
        confetti1.Stop();
        confetti2.Stop();

    }

    public void TryAgain()
    {
        ClearBoard();
        enabled = true;
        confetti1.Stop();
        confetti2.Stop();
    }

    public void HintButton()
    {
        LeanTween.moveLocalX(answerText, 0, 0.5f);
        LeanTween.moveLocalX(answerText, 202, 0.5f).setDelay(3f);
    }
}
