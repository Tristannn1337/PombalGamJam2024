using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpots : MonoBehaviour
{
    [SerializeField] List<Transform> topLeftHidingSpots = null;
    [SerializeField] List<Transform> topRightHidingSpots = null;
    [SerializeField] List<Transform> bottomLeftHidingSpots = null;
    [SerializeField] List<Transform> bottomRightHidingSpots = null;

    public Transform GetHidingSpot(Quadrant enemyQuadrant)
    {
        List<Transform> quadrantsToChooseFrom = new List<Transform>();

        if(enemyQuadrant != Quadrant.TopLeft)
        {
            quadrantsToChooseFrom.AddRange(topLeftHidingSpots);
        }
        if(enemyQuadrant != Quadrant.TopRight)
        {
            quadrantsToChooseFrom.AddRange(topRightHidingSpots);
        }
        if(enemyQuadrant != Quadrant.BottomLeft)
        {
            quadrantsToChooseFrom.AddRange(bottomLeftHidingSpots);
        }
        if(enemyQuadrant != Quadrant.BottomRight)
        {
            quadrantsToChooseFrom.AddRange(bottomRightHidingSpots);
        }

        return quadrantsToChooseFrom[Random.Range(0, quadrantsToChooseFrom.Count)];

    }
}
public enum Quadrant
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}
