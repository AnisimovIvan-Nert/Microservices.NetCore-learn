namespace Microservices.NetCore.Tests.Utilities;

public static class Categories
{
    public const string TraitName = "Category";
    
    public static class Integration
    {
        public const string Base = nameof(Integration);
        
        public const string Docker = $"{nameof(Integration)}.{nameof(Docker)}";
        public const string InMemoryWebApp = $"{nameof(Integration)}.{nameof(InMemoryWebApp)}";
    }
}