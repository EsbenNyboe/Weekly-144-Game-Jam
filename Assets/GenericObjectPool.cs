using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> : MonoBehaviour where T: Component
{
    [SerializeField]
    T prefab;

    Queue<T> objects = new Queue<T>();
    protected List<T> allObjects = new List<T>();
    public T GetObject()
    {
        if (objects.Count == 0)
        {
            AddObject();
        }
        T newObject = objects.Dequeue();
        newObject.gameObject.SetActive(true);
        return newObject;
    }

    public virtual void AddObject()
    {
        T MadeObject = Instantiate(prefab);
        objects.Enqueue(MadeObject);
        //allObjects.Add(MadeObject);
    }

    public virtual void ReturnObject(T DoneObject)
    {
        DoneObject.gameObject.SetActive(false);
        objects.Enqueue(DoneObject);
    }

}
