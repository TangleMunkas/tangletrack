using UnityEngine;

public class MenuSelectorManager : MonoBehaviour
{
    public GameObject ui_ManagerObject;
    private UI_Manager ui_Manager;

    private void Awake()
    {
        ui_Manager = ui_ManagerObject.GetComponent<UI_Manager>();
    }

    public void StoreButtonClicked()
    {
        ui_Manager.ChangeMenuToStore();
    }

    public void CustomizeButtonClicked()
    {
        ui_Manager.ChangeMenuToCosmetics();
    }

    public void HomeButtonClicked()
    {
        ui_Manager.ChangeMenuToMain();
    }

    public void FriendsButtonClicked()
    {
        ui_Manager.ChangeMenuToFriends();
    }

    public void LeaderBoardButtonClicked()
    {
        ui_Manager.ChangeMenuToLeaderBoard();
    }
}
