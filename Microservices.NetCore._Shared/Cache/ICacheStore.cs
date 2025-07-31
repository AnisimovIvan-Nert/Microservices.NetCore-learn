namespace Microservices.NetCore.Shared.Cache;

public interface ICacheStore  
{
  void Add(string key, object value, TimeSpan timeToLive);
  object? TryGet(string key);
}