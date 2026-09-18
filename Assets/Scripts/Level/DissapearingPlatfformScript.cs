using UnityEngine;
using System.Collections;

public class DissapearingPlatfformScript : MonoBehaviour
{
    [SerializeField] private GameObject Plattform;
    [SerializeField] private GameObject Marker;
    [SerializeField] private float Timer1 = 3;
    [SerializeField] private float Timer2;
    [SerializeField] private bool StartOff;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Timer2 == 0)
            Timer2 = Timer1;
        if (StartOff == true)
        Plattform.active = false;

        if (Marker != null)
        Marker.transform.position = Plattform.transform.position;

        StartCoroutine(DissapearTimer());
    }

    IEnumerator DissapearTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Timer1);

            Plattform.active = Plattform.active == true ? false : true;

            yield return new WaitForSeconds(Timer2);

            Plattform.active = Plattform.active == true ? false : true;
        }
        
    }

}
