using UnityEngine;
using UnityEngine.AI;

public class unitAgent : MonoBehaviour
{
    public Vector3 destination { get; private set; }
    private NavMeshPath path;
    private int currentWaypoint = 0;

    public float stoppingDistance;
    public float speed;

    // Parâmetros de separação
    public float separationRadius = 1.5f; // Raio para detectar agentes próximos
    public float separationStrength = 2f; // Força para se afastar de outros agentes
    public LayerMask agentLayer; // Camada para identificar outros agentes

    void Start()
    {
        path = null;
    }

    void Update()
    {
        if (path != null)
        {
            if (currentWaypoint < path.corners.Length)
            {
                Vector3 targetPosition = path.corners[currentWaypoint];
                float distance = Vector3.Distance(transform.position, targetPosition);

                // Move em direção ao próximo waypoint
                if (distance > stoppingDistance)
                {
                    Vector3 direction = (targetPosition - transform.position).normalized;

                    // Aplica a força de separação
                    Vector3 separationForce = CalculateSeparationForce();
                    direction += separationForce; // Ajusta a direção final

                    // Move a unidade
                    transform.position += direction.normalized * speed * Time.deltaTime;

                    // Rotação para olhar na direção do movimento
                    transform.LookAt(new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z + direction.z));
                }
                else
                {
                    currentWaypoint++; // Vai para o próximo waypoint
                }
            }
            else
            {
                Vector3 separationForce = CalculateSeparationForce();
                transform.position+= separationForce;
                path = null; // Chegou ao destino
            }
        }
    }

    public bool SetDestination(Vector3 position)
    {
        path = null;
        path = new NavMeshPath();
        currentWaypoint = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, Terrain.activeTerrain.terrainData.size.magnitude, NavMesh.AllAreas);
        Vector3 separationForce = CalculateSeparationForce();
        destination = hit.position + separationForce;
        return NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
    }

    public void ResetPath()
    {
        path = null;
    }

    public bool HasPath()
    {
        return (path != null);
    }

    /// <summary>
    /// Calcula a força de separação para evitar outros agentes próximos.
    /// </summary>
    private Vector3 CalculateSeparationForce()
    {
        Collider[] nearbyAgents = Physics.OverlapSphere(transform.position, separationRadius, agentLayer);
        Vector3 separationForce = Vector3.zero;

        foreach (Collider otherAgent in nearbyAgents)
        {
            if (otherAgent.gameObject != gameObject) // Ignora si mesmo
            {
                Vector3 directionAway = transform.position - otherAgent.transform.position;
                float distance = directionAway.magnitude;

                // Força inversamente proporcional à distância
                separationForce += directionAway.normalized / Mathf.Max(distance, 0.01f);
            }
        }

        return separationForce * separationStrength;
    }
}
