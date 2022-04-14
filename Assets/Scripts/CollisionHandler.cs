
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelDelay = 1f;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip crashSound;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem crashParticles;
    

    AudioSource audioSource;

    bool isTransitioning = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {

        if (isTransitioning)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Standing on the start");
                break;
            case "Finish":
                isTransitioning = true;
                StartWinSequence();
                break;
            default:
                isTransitioning = true;
                StartCrashSequence();
                break;
        }

    }
    void StartWinSequence()
    {
        winParticles.Play();
        audioSource.Stop();
        audioSource.volume = 1f;
        audioSource.PlayOneShot(winSound);
        GetComponent<Movement>().enabled = false;
        Invoke("NextLevel", levelDelay);
    }
    void StartCrashSequence()
    {
        transform.Find("SpaceRocket").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("SpaceRocket").transform.Find("Flap1").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("SpaceRocket").transform.Find("Flap2").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("SpaceRocket").transform.Find("Flap3").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("SpaceRocket").transform.Find("Flap4").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("Main Booster").gameObject.SetActive(false);
        transform.Find("Left Booster").gameObject.SetActive(false);
        transform.Find("Right Booster").gameObject.SetActive(false);
        crashParticles.Play();
        audioSource.Stop();
        audioSource.volume = 1f;
        audioSource.PlayOneShot(crashSound);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelDelay);

    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
