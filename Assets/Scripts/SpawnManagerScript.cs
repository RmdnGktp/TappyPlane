using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManagerScript : MonoBehaviour
{
    [SerializeField] GameObject [] rocks;
    [SerializeField] float spawnRate;
    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;

    void Start()
    {
        InvokeRepeating ("spawnTopRock", 0f, spawnRate);
        InvokeRepeating ("spawBottomRock", 2f, spawnRate);
    }

    void spawnTopRock ()
    {
        float Height = Random.Range(minHeight,maxHeight);
        Instantiate(rocks[0], new Vector3(transform.position.x, Height, transform.position.z), transform.rotation, gameObject.transform);
    }
 
    void spawBottomRock ()
    {
        float Height = Random.Range(-minHeight,-maxHeight);
        Instantiate(rocks[1], new Vector3(transform.position.x, Height, transform.position.z), transform.rotation, gameObject.transform);
    }


    void spawnFuel ()
    {
        
    }

    [ContextMenu ("cancelSpawn")]
    public void  cancelSpawn ()
    {
        CancelInvoke();
    }

}
