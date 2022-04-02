using UnityEngine;

namespace VomitCats
{
    enum CatState
    {
        Idle,
        Walk,
        Vomit
    }

    enum CleanerState
    {
        Idle,
        Walk
    }

    struct BaseStats
    {
        public Vector3 scale;
        public Vector3 position;
    }
}
