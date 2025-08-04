namespace Microservices.NetCore.Tests.Utilities.Scripts;

public static class ScriptUtilities
{
    private const string ProjectFolder = "Microservices.NetCore.Tests.Utilities.Scripts";
    private const string ScriptFolder = "scripts";
    private const int OuterFolderCount = 4;
    
    public static string GetScriptPath(ScriptSource source, ScriptTarget target, ScriptAction action)
    {
        var projectFolder = ProjectFolder;
        for (var i = 0; i < OuterFolderCount; i++)
        {
            projectFolder = Path.Combine("..", projectFolder);
        }

        var scriptFolder = Path.Combine(projectFolder, ScriptFolder);
        
        var sourceFolder = Path.Combine(scriptFolder, source.ToString());
        var targetFolder = Path.Combine(sourceFolder, target.ToString());

        var fileName = action.ToString();
        var fileExtension = target switch
        {
            ScriptTarget.MySql => ".sql",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

        return Path.Combine(targetFolder, fileName + fileExtension);
    }
}

public enum ScriptSource
{
    EventStore
}
    
public enum ScriptTarget
{
    MySql
}
    
public enum ScriptAction
{
    CreateTables,
    DropTables
}