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

    }

    public void HomeButtonClicked()
    {
        ui_Manager.ChangeMenuToMain();
    }

    public void FriendsButtonClicked()
    {

    }

    public void LeaderBoardButtonClicked()
    {

    }
}
