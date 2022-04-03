using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace VomitCats
{
    public class DrawSorting : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private List<IDrawSort> objectList;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            objectList = new List<IDrawSort>();
        }

        private void Start()
        {
            objectList.Add(FindObjectOfType<Cleaner>());
            objectList.AddRange(FindObjectsOfType<Cat>().ToList());
        }

        private void FixedUpdate()
        {
            var sortObjectList = objectList.OrderByDescending(e => e.GetPositionY()).ToList();

            for (int i = 0; i < sortObjectList.Count; i++)
            {
                sortObjectList[i].SetDrawOrder(i * 10);
            }
        }
        #endregion


    }
}
