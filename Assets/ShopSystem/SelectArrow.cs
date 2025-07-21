using UnityEngine;

public class SelectArrow : MonoBehaviour
{


    public sideType side;
    public enum sideType
    {
        left,
        right,

    }
    public void loadNextCharacter() // assigned in the editor
    {
        switch (side)
        {
            case sideType.left:
                ShopSystem.instance.triggerOnLeftSelected();
                break;
            case sideType.right:
                ShopSystem.instance.triggerOnRightSelected();
                break;
        }

    }
}