using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlattformScript : MonoBehaviour
{
    [SerializeField] private GameObject Plattform;
    [SerializeField] private GameObject Point1;
    [SerializeField] private GameObject Point2;
    [SerializeField] private float Speed = 10;
    [SerializeField] private float Wait = 1;
    [SerializeField] private float Wait2 = -1;
    [SerializeField] private float DeSync = 0;
    [SerializeField] private bool OneWay = false;

    private Vector3 TargetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Wait2 == -1)
            Wait2 = Wait;
        Plattform.transform.position = Point1.transform.position;
        TargetPosition = Point2.transform.position;
        StartCoroutine(MovePlattform());

    }

    IEnumerator MovePlattform()
    {
        yield return new WaitForSeconds(DeSync);

        while (true)
        {
            while ((TargetPosition - Plattform.transform.position).sqrMagnitude > 0.01f)
            {
                Plattform.transform.position = Vector3.MoveTowards(Plattform.transform.position, TargetPosition, Speed * Time.deltaTime);
                yield return null;
            }

            if (OneWay == false)
            {
                TargetPosition = TargetPosition == Point1.transform.position ? Point2.transform.position : Point1.transform.position;
            }
            else if (OneWay == true)
            {
                yield return new WaitForSeconds(Wait2);

                // Temporarily detach any player standing on the platform
                foreach (Transform child in Plattform.transform)
                {
                    if (child.CompareTag("Player"))
                        child.SetParent(null, true);
                }

                // Teleport platform back
                Plattform.transform.position = Point1.transform.position;
            }
            yield return new WaitForSeconds(Wait);
        }
    }
}
