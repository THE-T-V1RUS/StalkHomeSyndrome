using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float roomCompression = -40f;
    GameManager gameManager;
    bool isInitialized;

    private void Start()
    {
        SetCompressorThreshold(roomCompression);
        Initialize();
    }

    void Initialize()
    {
        gameManager = GameManager.Instance;
        if(gameManager == null)
        {
            Initialize();
            return;
        }

        isInitialized = true;
    }


    private void Update()
    {
        if (isInitialized)
        {
            if(gameManager.currentRoom == CurrentRoom.LivingRoom)
                SetCompressorThreshold(0f);
            else
                SetCompressorThreshold(roomCompression);
        }
    }

    public void SetCompressorThreshold(float value)
    {
        // Example: value in dB, e.g., -20f
        audioMixer.SetFloat("LivingRoom_CompressorThreshold", value);
    }
}
