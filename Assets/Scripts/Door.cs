using System.Collections;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Key key;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text winText;

    private bool _isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerInventory.HasKey && !_isTriggered)
            {
                _isTriggered = true;
                key.SpawnKey();
                objectiveText.text = "Objective: Find the key";
            }

            else if (playerInventory.HasKey)
            {
                winText.gameObject.SetActive(true);
                winText.text = "You Win!\nThanks for playing <3";

                StartCoroutine(Win());
            }
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadSceneAsync("MainMenu");
    }
}
