using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private TMP_Text loseText;

    private AudioSource _audioSource;
    private float _currentHealth;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _currentHealth = maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = _currentHealth;
    }

    private IEnumerator Die()
    {
        loseText.gameObject.SetActive(true);
        loseText.text = "You Died!";

        if (InputManager.Instance != null)
            InputManager.Instance.TurnOffInputs();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;
        healthBarSlider.value = _currentHealth;
        _audioSource.PlayOneShot(takeDamageClip);

        if (_currentHealth <= 0f)
        {
            StartCoroutine(Die());
        }
    }
}
