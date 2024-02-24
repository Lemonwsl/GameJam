using System;
using System.Collections.Generic;
using System.Linq;

public class ServiceLocator : Singleton<ServiceLocator>
{
    private readonly Dictionary<string, List<IGameService>> services = new ();
    public List<T> Get<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            throw new InvalidOperationException($"No services registered for type {key}.");
        }

        // Attempt to cast each service to the desired type and return the list
        return services[key].OfType<T>().ToList();
    }
    public void Register<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if (!this.services.ContainsKey(key))
        {
            this.services[key] = new List<IGameService>();
        }

        this.services[key].Add(service);
    }
    
    public void Unregister<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if (!this.services.ContainsKey(key))
        {
            return;
        }

        this.services[key].Remove(service);
        if (this.services[key].Count == 0)
        {
            this.services.Remove(key);
        }
    }
}