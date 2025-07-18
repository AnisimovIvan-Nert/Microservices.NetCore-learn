using Dynamitey;

namespace Microservices.NetCore.Nancy.Shared;

public class QueryParameter<T>(string name, string typeName)
{
    public T Get(dynamic parameters)
    {
        return Dynamic.InvokeGet(parameters, name);
    }
    
    public override string ToString()
    {
        return $"{{{name}:{typeName}}}";
    }
}