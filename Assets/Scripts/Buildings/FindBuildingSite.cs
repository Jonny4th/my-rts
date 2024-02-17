using UnityEngine;

public class FindBuildingSite : MonoBehaviour
{
    [SerializeField]
    private bool canBuild = false;
    public bool CanBuild { get { return canBuild; } set { canBuild = value; } }

    [SerializeField]
    private MeshRenderer[] modelRdr;
    [SerializeField]
    private MeshRenderer planeRdr;

    private readonly Color32 GhostGreen = new Color32(0, 255, 0, 100);
    private readonly Color32 GhostRed = new Color32(255, 0, 0, 100);


    void Start()
    {
        //Setup Building Color
        for (int i = 0; i < modelRdr.Length; i++)
            modelRdr[i].material.color = GhostGreen;

        //Setup Plane Color
        planeRdr.material.color = GhostGreen;

        CanBuild = true;
    }

    private void SetCanBuild(bool flag)
    {
        if (flag)
        {
            for (int i = 0; i < modelRdr.Length; i++)
                modelRdr[i].material.color = new Color32(0, 255, 0, 50);

            planeRdr.material.color = GhostGreen;
            canBuild = true;
        }
        else
        {
            for (int i = 0; i < modelRdr.Length; i++)
                modelRdr[i].material.color = GhostRed;

            planeRdr.material.color = GhostRed;
            canBuild = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource") || other.CompareTag("Building") || other.CompareTag("Unit"))
            SetCanBuild(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Resource") || other.CompareTag("Building") || other.CompareTag("Unit"))
            SetCanBuild(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Resource") || other.CompareTag("Building") || other.CompareTag("Unit"))
            SetCanBuild(true);
    }
}
