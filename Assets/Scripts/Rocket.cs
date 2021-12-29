using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    [SerializeField]float rotationUnit = 100f;
    [SerializeField]float thrustUnit = 100f;
    [SerializeField]AudioClip engineSound;
    [SerializeField]AudioClip explosionSound;
    [SerializeField]AudioClip successfulSound;
    [SerializeField]ParticleSystem thrustingEffect;
    [SerializeField]ParticleSystem explosionEffect;
    [SerializeField]ParticleSystem successfulEffect;
    Rigidbody rigidBody;
    GameObject rocketLightObject;
    Light rocketLight;
    AudioSource audioSource;
    enum States {Alive, Died, Transcending}
    States state;
    bool collisionsEnabled = false;
    [SerializeField]float levelLoadDelay = .5f;
    void Start()
    {
        rocketLightObject = transform.Find("Rocket Light").gameObject;
        rocketLight = rocketLightObject.GetComponent<Light>();
        state = States.Alive;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(state == States.Alive){
            Thrusting();
            Rotating();
            rigidBody.angularVelocity = rigidBody.angularVelocity * .8f;
        }
        if(Debug.isDebugBuild){
            RespondToDebugKeys();
        }
    }
    void RespondToDebugKeys(){
        if(Input.GetKeyDown(KeyCode.L)){
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C)){
            collisionsEnabled = !collisionsEnabled;
        }
    }
    void OnCollisionEnter(Collision collision){
        if(state != States.Alive || collisionsEnabled){
            return;
        }
        switch(collision.gameObject.tag){
            case "Friendly":
                break;
            case "Finish":
                successSequence();
                break;
            default:
                deathSequence();
                break;
        }
    }
    void successSequence(){
        state = States.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successfulSound);
        thrustingEffect.Stop();
        successfulEffect.Play();
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }
    void deathSequence(){
        state = States.Died;
        audioSource.Stop();
        audioSource.PlayOneShot(explosionSound);
        thrustingEffect.Stop();
        explosionEffect.Play();
        rocketLight.intensity = 0;
        Invoke(nameof(LoadFirstLevel), levelLoadDelay);
    }
    void LoadNextLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(SceneManager.sceneCountInBuildSettings - 1 == currentSceneIndex){
            LoadFirstLevel();
        }
        else{
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
    void LoadFirstLevel(){
        SceneManager.LoadScene(0);
    }
    void Thrusting(){
        if(Input.GetKey(KeyCode.Space)){
            ApplyThrusting();
        }
        else{
            audioSource.Stop();
            thrustingEffect.Stop();
        }
    }
    void ApplyThrusting(){
        float thrustValue = thrustUnit * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustValue);
        if(!audioSource.isPlaying){
            audioSource.PlayOneShot(engineSound);
        }
        if(!thrustingEffect.isPlaying){
            thrustingEffect.Play();
        }
    }
    void Rotating(){
        float rotationValue = rotationUnit * Time.deltaTime;
        if(Input.GetKey(KeyCode.A)){
            transform.Rotate(Vector3.forward * rotationValue);
        }
        else if(Input.GetKey(KeyCode.D)){
            transform.Rotate(-Vector3.forward * rotationValue);
        }
    }
}
