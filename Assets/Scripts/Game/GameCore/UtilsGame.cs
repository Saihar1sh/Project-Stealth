using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace ShGames.Utils.PathFinding
{
    public static class PathfindingUtils
    {
        //public NavMeshAgent agent;
        //public LineRenderer lineRenderer;
        private const float Speed = 3.5f;
        private static List<Vector3> smoothedPath;
        private static int currentPointIndex = 0;

        // Catmull-Rom spline interpolation
        private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            Vector3 result = 0.5f * ((2f * p1) +
                                     (-p0 + p2) * t +
                                     (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
                                     (-p0 + 3f * p1 - 3f * p2 + p3) * t3);

            return result;
        }

        // Smooth path calculation
        public static List<Vector3> SmoothPath(Vector3[] pathCorners, int subdivisions)
        {
            List<Vector3> smoothPath = new List<Vector3>();

            for (int i = 0; i < pathCorners.Length - 1; i++)
            {
                Vector3 p0 = i == 0 ? pathCorners[i] : pathCorners[i - 1];
                Vector3 p1 = pathCorners[i];
                Vector3 p2 = pathCorners[i + 1];
                Vector3 p3 = (i + 2 < pathCorners.Length) ? pathCorners[i + 2] : p2;

                for (int j = 0; j <= subdivisions; j++)
                {
                    float t = j / (float)subdivisions;
                    Vector3 point = CatmullRom(p0, p1, p2, p3, t);
                    smoothPath.Add(point);
                }
            }

            return smoothPath;
        }

        /*void Update()
        {
            if (agent.hasPath && smoothedPath == null)
            {
                Vector3[] corners = agent.path.corners;
                smoothedPath = SmoothPath(corners, 10);
    
                // For visualization
                lineRenderer.positionCount = smoothedPath.Count;
                lineRenderer.SetPositions(smoothedPath.ToArray());
            }
    
            // Move the agent along the smoothed path
            if (smoothedPath != null && currentPointIndex < smoothedPath.Count)
            {
                Vector3 targetPosition = smoothedPath[currentPointIndex];
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, targetPosition, Speed * Time.deltaTime);
    
                // When agent reaches the current target point, move to the next
                if (Vector3.Distance(agent.transform.position, targetPosition) < 0.1f)
                {
                    currentPointIndex++;
                }
            }
        }*/
    }
}