using UnityEngine;

public class ShowWindowComponent : MonoBehaviour
{
    public void Show()
    {
        WindowUtils.CreateWindow(LoadPaths.Resources.ManagePerksWindowPath);
    }
}
