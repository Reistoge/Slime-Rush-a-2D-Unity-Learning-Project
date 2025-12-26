using UnityEngine;

public class CharacterSelector : MonoBehaviour
{


    //this will hold the characters
    // Start is called before the first frame update
    // I HAVE TO CREATE A THING THAT JUST HOLD THE CHARACTERS
    [SerializeField] private GameObject[] PlayersToSelect;
    [SerializeField] private GameObject PlayerLeft;
    [SerializeField] private GameObject PlayerRight;
    private int posRight = 1;
    private int posLeft = 0;


    private int _index;

    public GameObject[] PlayersToSelect1 { get => PlayersToSelect; set => PlayersToSelect = value; }
    public GameObject PlayerRight1 { get => PlayerRight; set => PlayerRight = value; }
    public GameObject PlayerLeft1 { get => PlayerLeft; set => PlayerLeft = value; }
    public int PosRight { get => posRight; set => posRight = value; }
    public int PosLeft { get => posLeft; set => posLeft = value; }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            _index = GameManager.Instance.Char_Index;

        }
        else
        {
            _index = 0;
        }

    }

    void Update()
    {

        // char index will change so we communicate between gamemanager getindexname and characterselectorscript
        // we update char index and we assign the player in the array 

        _index = GameManager.Instance.Char_Index;
        GameManager.Instance.SelectedPlayer = PlayersToSelect[_index];





    }




}
