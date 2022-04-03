using UnityEngine;

namespace VomitCats
{
    public enum CatState
    {
        Idle,
        Walk,
        Vomit
    }

    public enum CleanerState
    {
        Idle,
        Walk
    }

    public enum Language
    {
        Russian,
        English
    }

    public struct BaseStats
    {
        public Vector3 scale;
        public Vector3 position;
    }

    interface IDrawSort
    {
        public float GetPositionY();
        public void SetDrawOrder(int order);
    }
}
