﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private Vector3[] nodes;
    private int nodeCount;

    // Use this for initialization
    void Start()
    {
        nodeCount = transform.GetChildCount();
        nodes = new Vector3[nodeCount];

        for (int i = 0; i < nodeCount; i++)
        {
            nodes[i] = transform.GetChild(i).position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nodeCount > 1)
        {
            for (int i = 0; i < nodeCount - 1; i++)
            {
                Debug.DrawLine(nodes[i], nodes[i + 1], Color.green);

            }
        }
    }
    public Vector3 ProjectPositionOnRail(Vector3 pos)
    {
        int closestNodeIndex = GetClosestNode(pos);//Recebe o node mais proximo

        if (closestNodeIndex == 0)//projeta no primeiro segmento
        {
            return ProjectOnSegment(nodes[0], nodes[1], pos);
        }
        else if (closestNodeIndex == nodeCount - 1) //Projeta no ultimo segmento
        {
            return ProjectOnSegment(nodes[nodeCount - 1], nodes[nodeCount - 2], pos);
        }
        else//projeta nos dois segmentos continuos, retorna mais proximo
        {
            Vector3 leftSeg = ProjectOnSegment(nodes[closestNodeIndex - 1], nodes[closestNodeIndex], pos);
            Vector3 rightSeg = ProjectOnSegment(nodes[closestNodeIndex + 1], nodes[closestNodeIndex], pos);

            Debug.DrawLine(pos, leftSeg, Color.red);
            Debug.DrawLine(pos, rightSeg, Color.blue);

            if ((pos - leftSeg).sqrMagnitude <= (pos - rightSeg).sqrMagnitude)
            {
                return leftSeg;
            }
            else
            {
                return rightSeg;
            }

        }
    }

    private int GetClosestNode(Vector3 pos)
    {
        int closestNodeIndex = -1;
        float shortestDistane = 0.0f;

        for (int i = 0; i < nodeCount; i++)
        {
            float sqrDistance = (nodes[i] - pos).sqrMagnitude;
            if (shortestDistane == 0.0f || sqrDistance < shortestDistane)
            {
                shortestDistane = sqrDistance;
                closestNodeIndex = i;

            }
        }
        return closestNodeIndex;
    }

    public Vector3 ProjectOnSegment(Vector3 v1, Vector3 v2, Vector3 pos)
    {
        Vector3 v1ToPos = pos - v1;
        Vector3 segDirection = (v2 - v1).normalized;

        float distanceFromV1 = Vector3.Dot(segDirection, v1ToPos);
        if (distanceFromV1 < 0.0f)
        {
            return v1;
        }
        else if (distanceFromV1 * distanceFromV1 > (v2 - v1).sqrMagnitude)
        {
            return v2;
        }
        else
        {
            Vector3 fromV1 = segDirection * distanceFromV1;
            return v1 + fromV1;
        }
    }
}

