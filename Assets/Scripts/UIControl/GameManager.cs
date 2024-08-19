using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction pauseAction;
    [SerializeField]
    private GameObject pauseMenuPanel;
    [SerializeField]
    public bool isPaused = false;
    int index;

    [Header("Volume")]
    [SerializeField]
    public Slider volumeSlider;
    [SerializeField]
    public AudioMixer mixer;
    [SerializeField]
    private TextMeshProUGUI volumeText;
    //private TMP_Text volumeText;
    private float value;
    [SerializeField]
    private GameObject _confirmationPrompt;
    float volumeSiliderValue = 20;


    private void Awake()
    {

        /*int currentSceneIndex = currentScene.buildIndex;
        if (currentSceneIndex == 0)
        {
            AudioManager.instance.Play("MainMenu");
        }*/
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
        pauseAction.performed += TogglePause;
    }

    private void OnDestroy()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= TogglePause;
        }
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        
        isPaused = true;
        
    }


    private void Start()
    {
        Time.timeScale = 1f;
        volumeSlider.value = PlayerPrefs.GetFloat("volume", volumeSlider.value);
        volumeText.text = volumeSiliderValue.ToString("0");
        Debug.Log(volumeSlider.value);
        // Lấy scene hiện tại
        Scene currentScene = SceneManager.GetActiveScene();

        // Lấy giá trị thứ tự của scene hiện tại
        int currentSceneIndex = currentScene.buildIndex;

        if(currentSceneIndex > 0)
        {
            AudioManager.instance.Stop("MainMenu");
        }
        else if(currentSceneIndex == 0)
        {
            AudioManager.instance.Play("MainMenu");
        }

        /*mixer.GetFloat("volume", out value);
        volumeSlider.value = value;*/
    }


    private void Update()
    {
        //volumeText.text = value.ToString();

    }

    public void LoadScene(int numberofscene)
    {
        index = numberofscene;
        SceneManager.LoadScene(index);
        if(index != 0)
        {
            AudioManager.instance.Stop("MainMenu");
        }
        
    }

    public void SetVolume()
    {
        mixer.SetFloat("volume", volumeSlider.value);
        volumeSiliderValue = volumeSlider.value /*+ 80f*/;
        volumeText.text = volumeSiliderValue.ToString("0");
        /*AudioListener.volume = volume;
        volumeText.text = volume.ToString("0.0");*/
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        StartCoroutine(ConfirmationBox());
        Debug.Log(volumeSlider.value);
    }


    private IEnumerator ConfirmationBox()
    {
        _confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        _confirmationPrompt.SetActive(false);
    }
}
