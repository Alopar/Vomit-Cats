using System.Collections.Generic;
using UnityEngine;

namespace VomitCats
{
    [CreateAssetMenu(fileName = "CatTalk", menuName = "CatTalks")]
    public class Talks : ScriptableObject
    {
        public Language Language;
        public List<string> texts;

        public string GetRandomMessage()
        {
            return texts[Random.Range(0, texts.Count)];
        }
    }
}