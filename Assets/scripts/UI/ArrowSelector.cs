using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ArrowSelector : MonoBehaviour
{
    [SerializeField]
    GameObject Characters_pool;
    [SerializeField] GameObject Character_left;
    [SerializeField] GameObject Character_right;




    private void OnEnable()
    {
        Character_left.GetComponent<Image>().sprite = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1[0].GetComponentInChildren<SpriteRenderer>().sprite;
        
        Character_right.GetComponent<Image>().sprite = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1[1].GetComponentInChildren<SpriteRenderer>().sprite;
    }
    private void Start()
    {

        //Character_left.GetComponent<Image>().sprite = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1[0].GetComponent<PlayerScript>().Sr.sprite.sprite;
        //;
        //Character_right.GetComponent<Image>().sprite = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1[1].GetComponent<PlayerScript>().Sr.sprite.sprite; 
    }

    // Start is called before the first frame update
    public void RightButton()
    {
        // character right and left are the gameobjects(buttons) that the player can select.


        // so the character pool has the players and the pos 
        int selector_length = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1.Length;

        // check the index to dont have errors
        if (Characters_pool.GetComponent<CharacterSelector>().PosRight + 2 <= selector_length)
        {
            // we sum the pos of the right and left pivot
            Characters_pool.GetComponent<CharacterSelector>().PosLeft += 2;
            Characters_pool.GetComponent<CharacterSelector>().PosRight += 2;

            // we assign the name of the left and right to the new pivot to select
            Character_left.name = Characters_pool.GetComponent<CharacterSelector>().PosLeft.ToString();
            Character_right.name = (Characters_pool.GetComponent<CharacterSelector>().PosRight).ToString();

            //get the values from the characters pool array
            // the ints are to reduce the line of code 
            int PosLeft = Characters_pool.GetComponent<CharacterSelector>().PosLeft;
            int PosRight = Characters_pool.GetComponent<CharacterSelector>().PosRight;

            // we get the component with the image of the pool.
            SpriteRenderer leftImage = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1[PosLeft].GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer rightImage = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1[PosRight].GetComponentInChildren<SpriteRenderer>();

            // we change the sprite as well of the buttons of the same pos of the pool
            Character_left.GetComponent<Image>().sprite = leftImage.sprite;
            Character_right.GetComponent<Image>().sprite = rightImage.sprite;

        }






    }
    public void LeftButton()
    {

        int selector_length = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1.Length;
        if (Characters_pool.GetComponent<CharacterSelector>().PosLeft - 2 >= 0)
        {
            Characters_pool.GetComponent<CharacterSelector>().PosLeft -= 2;
            Characters_pool.GetComponent<CharacterSelector>().PosRight -= 2;
            Character_left.name = Characters_pool.GetComponent<CharacterSelector>().PosLeft.ToString();
            Character_right.name = (Characters_pool.GetComponent<CharacterSelector>().PosRight).ToString();
            int PosLeft = Characters_pool.GetComponent<CharacterSelector>().PosLeft;
            int PosRight = Characters_pool.GetComponent<CharacterSelector>().PosRight;
            SpriteRenderer leftImage = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1[PosLeft].GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer rightImage = Characters_pool.GetComponent<CharacterSelector>().PlayersToSelect1[PosRight].GetComponentInChildren<SpriteRenderer>();

            Character_left.GetComponent<Image>().sprite = leftImage.sprite;
            Character_right.GetComponent<Image>().sprite = rightImage.sprite;
        }


        //Character_left.GetComponent<Image>().sprite = leftImage.sprite;
        //Character_right.GetComponent<Image>().sprite = rightImage.sprite;

    }
}
