using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject ShipGroup;

    public void OnBuyButtonClick()
    {
        ShipGroup.SetActive(true);
        Invoke("SceneChange", 3.0f);
    }

    void SceneChange()
    {
        SceneManager.LoadScene("UIScene");
        SceneManager.LoadScene("ARScene", LoadSceneMode.Additive);

    }
}
