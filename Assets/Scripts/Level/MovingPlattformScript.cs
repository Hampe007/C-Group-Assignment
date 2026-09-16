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
    [SerializeField] private float DeSync = 0;

    private Vector3 TargetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

            TargetPosition = TargetPosition == Point1.transform.position 
                ? Point2.transform.position : Point1.transform.position;

            yield return new WaitForSeconds(Wait);
        }
    }
}
