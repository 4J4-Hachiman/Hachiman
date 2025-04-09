using UnityEngine;
using UnityEngine.AI;

public interface IMoveable
{
    NavMeshAgent Agent { get; set;}
    
    /// <summary>
    /// Met à jour la destination de l'ennemi.
    /// </summary>
    /// <param name="position">Destination du joueur.</param>
    /// <returns>void</returns>
    void SetNavDestination(Vector3 position);

    /// <summary>
    /// Set la vitesse de déplacement de l'ennemi.
    /// </summary>
    /// <param name="vitesse">Vitesse de déplacement.</param>
    /// <returns>void</returns>
    void SetNavVitesse(float vitesse);
}
