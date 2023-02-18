using DG.Tweening;
using UnityEngine;

public class DoorIndicator : MonoBehaviour
{
    public MeshRenderer[] indicatorMesh;
    public Material greenMaterial;
    public Material yellowMaterial;
    public Material redMaterial;

    public enum Colors{
        RED, YELLOW, GREEN
    };

	private void Start()
	{
		SetColor(Colors.YELLOW);
	}

    public void SetColor(Colors color) {

        foreach (var item in indicatorMesh)
        {
            switch(color) {
                case Colors.RED: 
                    item.material = redMaterial;
                    break;
                case Colors.GREEN: 
                    item.material = greenMaterial;
                    break;
                case Colors.YELLOW: 
                default: 
                    item.material = yellowMaterial;
                    break;
            }
        }


    }
}