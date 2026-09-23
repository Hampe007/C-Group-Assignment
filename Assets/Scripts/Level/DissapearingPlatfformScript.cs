using UnityEngine;
using System.Collections;

public class DissapearingPlatfformScript : MonoBehaviour
{
    [SerializeField] private GameObject Plattform;
    [SerializeField] private GameObject Marker;
    [SerializeField] private float Timer1 = 3;
    [SerializeField] private float Timer2 = -1;
    [SerializeField] private bool StartOff;
    private Material MaterialOn;
    [SerializeField] private Material MaterialOff;
    [SerializeField] private GameObject Player;
    private bool IsOff = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaterialOn = Plattform.GetComponent<MeshRenderer>().material;
        //Checks if Timer2 is empty, if it is then it makes it equal to timer1.
        if (Timer2 == -1)
            Timer2 = Timer1;
        //Check if plattform should begin turned off and then does the necessary steps to turn it off.
        if (StartOff == true)
        {
            //Plattform.active = false;  || OLD CODE. IN CASE OF EMERGENCY BREAK // AND DELETE THE 3 LINES BELOW.
            IsOff = true;
           Plattform.GetComponent<Collider>().enabled = !IsOff;
            Plattform.GetComponent<MeshRenderer>().material = MaterialOff;
        }
        
        //Checks if there is a marker for the plattform or not. If there is one then it sets the markers position to be equal to the plattforms.
        if (Marker != null)
            Marker.transform.position = Plattform.transform.position;

        StartCoroutine(DissapearTimer());
    }

    //The big loop where it all happens.
    IEnumerator DissapearTimer()
    {
        while (true)
        {
            //Waits to turn on/off a predetermined amount of time.
            yield return new WaitForSeconds(Timer1);

            //Plattform.active = Plattform.active == true ? false : true;  || OLD CODE. IN CASE OF EMERGENCY BREAK // AND DELETE THE 3 LINES BELOW.
            
            //Sets a bool which decides if collision should be ignored or not.
            IsOff = IsOff == true ? false : true;
            //Sets of collision should be ignored between plattform and player depending on if the plattform should be off or not.
            Plattform.GetComponent<Collider>().enabled = !IsOff;
            //Changes the material to the opposite material.
            Plattform.GetComponent<MeshRenderer>().material = Plattform.GetComponent<MeshRenderer>().material == MaterialOn ? MaterialOff : MaterialOn;

            //The code below is the exact same as the one above, its just for Timer2 instead of Timer1. Im sure there is an easier way to handle
            //this but i decided to do it this way :3
            yield return new WaitForSeconds(Timer2);

            //Plattform.active = Plattform.active == true ? false : true;
            
            IsOff = IsOff == true ? false : true;
            Plattform.GetComponent<Collider>().enabled = !IsOff;
            Plattform.GetComponent<MeshRenderer>().material = Plattform.GetComponent<MeshRenderer>().material == MaterialOn ? MaterialOff : MaterialOn;
        }
        
    }

}
