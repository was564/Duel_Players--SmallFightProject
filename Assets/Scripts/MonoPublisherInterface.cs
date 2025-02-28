using System.Collections.Generic;
using UnityEngine;

public abstract class MonoPublisherInterface : MonoBehaviour
{
    private List<MonoObserverInterface> _observersList = new List<MonoObserverInterface>();
    
    public virtual void RegisterObserver(MonoObserverInterface observer)
    {
        _observersList.Add(observer);
    }
    
    public virtual void RemoveObserver(MonoObserverInterface observer)
    {
        _observersList.Remove(observer);
    }

    public virtual void Notify()
    {
        foreach (var observer in _observersList)
        {
            observer.Notify();
        }
    }
}