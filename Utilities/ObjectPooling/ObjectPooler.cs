using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T : Component
{
    private T prefab;
    private Transform parent;
    private readonly List<T> pool = new List<T>();

    public void Init(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            Add();
        }
    }

    public T Get()
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }

        return Add(); // Expand pool if all used
    }

    public void Return(T item)
    {
        item.gameObject.SetActive(false);
    }

    public T Add()
    {
        T instance = Object.Instantiate(prefab, parent);
        instance.gameObject.SetActive(false);
        pool.Add(instance);
        return instance;
    }

    public void Remove(T item)
    {
        if (pool.Contains(item))
        {
            pool.Remove(item);
            Object.Destroy(item.gameObject);
        }
    }

    public void Clear()
    {
        foreach (var item in pool)
        {
            if (item != null)
                Object.Destroy(item.gameObject);
        }

        pool.Clear();
    }
}
