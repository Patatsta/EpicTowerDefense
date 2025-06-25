using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/WaveData")]
public class WaveData : ScriptableObject
{
    public List<EnemyAmount> enemies;
}

