using UnityEngine;

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
    private string correctWord;


    [Header("States")]
    public Tile.State emptyState;
    public Tile.State filledState;
    public Tile.State correctState;
    public Tile.State wrongSpotState;
    public Tile.State incorrectState;


    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }


    private void Start()
    {
        SetCorrectWord();
    }


    private void Update()
    {
        Row currRow = rows[rowInd];
        
        //backspace pressed
        if(Input.GetKeyDown(KeyCode.Backspace))
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


    private void SubmitRow(Row row)
    {
        //we need 2 for loops cause we need to handle the edge case where our guess has
        //a letter 2 times or more but the correct word has it only once (or less number of times than the guess)
        //example correctWord is belts and you guess bells

        string alteredWord = correctWord;

        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile currTile = row.tiles[i];

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


        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile currTile = row.tiles[i];

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

        rowInd++;
        colInd = 0;

        if(rowInd >= rows.Length)
        {
            //gameover
            enabled = false;
        }

    }


    private void SetCorrectWord()
    {
        correctWord = APIHelper.GetNewWord().word;
        correctWord = correctWord.Trim();
    }
}
