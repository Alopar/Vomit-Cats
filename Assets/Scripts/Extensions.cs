using UnityEngine;

namespace VomitCats
{
    public enum HorizontalOrientation
    {
        Left,
        Right
    }

    public enum VerticalOrientation
    {
        Up,
        Bottom
    }

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

    public static class GameSettings
    {
        public static Language CurrentLanguage = Language.English;
        public static bool Mute = false;
    }
}
