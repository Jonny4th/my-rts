using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] woodTreePrefab;

    [SerializeField]
    private Transform woodTreeParent;

    [SerializeField]
    private ResourceSource[] resources;

    public static ResourceManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }

        Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        FindAllResource();
    }

    private void FindAllResource()
    {
        resources = FindObjectsOfType<ResourceSource>();
    }
}
