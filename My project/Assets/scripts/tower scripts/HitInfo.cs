using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInfo
{
    public struct HitData
    {
        public int damage;
        public GameObject tower;
    }
    public interface IDamageable
    {
        void TakeHit(HitData data);
    }
}
