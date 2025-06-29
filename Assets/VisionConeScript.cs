using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class FieldOfView2D : MonoBehaviour
{
    public float viewRadius = 5f;
    [Range(0, 360)] public float viewAngle = 90f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution = 1f;
    public int edgeResolveIterations = 4;
    public float edgeDstThreshold = 0.5f;

    public Vector2 facingDirection = Vector2.right; // This must be set externally
    public Transform parentTransform;
    public RandomNPCMovement parentMove;
    public Transform playerTransform;

    [SerializeField] private float castOffset = 2f; // 👈 new offset

    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    void Start()
    {
        viewMeshFilter = GetComponent<MeshFilter>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine(FindTargetsWithDelay(0.01f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        bool sawPlayerThisFrame = false;

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        foreach (var targetCollider in targetsInViewRadius)
        {
            Transform target = targetCollider.transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(facingDirection, dirToTarget) < viewAngle / 2)
            {
                Vector2 rayOrigin = (Vector2)transform.position + facingDirection.normalized * castOffset;
                float dstToTarget = Vector2.Distance(rayOrigin, target.position);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dirToTarget, dstToTarget, obstacleMask);

                // Debug ray to visualize cast
                Debug.DrawRay(rayOrigin, dirToTarget * dstToTarget, Color.green);

                if (!hit)
                {
                    visibleTargets.Add(target);
                    sawPlayerThisFrame = true;

                    if (!parentMove.playerSpotted)
                    {
                        parentMove.playerSpotted = true;
                        print("spotted!");
                    }

                    parentMove.spottedPlayerDirection = playerTransform.position;
                    break; // Stop after first visible target (optional)
                }
            }
        }

        if (!sawPlayerThisFrame && parentMove.playerSpotted)
        {
            parentMove.playerSpotted = false;
            print("lost sight!");
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = -viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector2.zero) viewPoints.Add(edge.pointA);
                    if (edge.pointB != Vector2.zero) viewPoints.Add(edge.pointB);
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float localAngle)
    {
        Vector2 dir = DirFromAngle(localAngle);
        Vector2 rayOrigin = (Vector2)transform.position + dir * castOffset;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, viewRadius, obstacleMask);

        if (hit)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, localAngle);
        }
        else
        {
            return new ViewCastInfo(false, rayOrigin + dir * viewRadius, viewRadius, localAngle);
        }
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector2 minPoint = Vector2.zero;
        Vector2 maxPoint = Vector2.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    public Vector2 DirFromAngle(float angleInDegrees)
    {
        float baseAngle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        float totalAngle = baseAngle + angleInDegrees;
        float rad = totalAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool hit, Vector2 point, float dst, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.dst = dst;
            this.angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector2 pointA;
        public Vector2 pointB;

        public EdgeInfo(Vector2 a, Vector2 b)
        {
            pointA = a;
            pointB = b;
        }
    }
}