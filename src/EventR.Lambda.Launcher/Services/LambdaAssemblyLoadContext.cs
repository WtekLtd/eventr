using System.Reflection;
using System.Runtime.Loader;

namespace EventR.Lambda.Launcher.Services;

public class LambdaAssemblyLoadContext(string assemblyPath) : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _assemblyDependencyResolver = new(assemblyPath);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (assemblyName.Name == "Amazon.Lambda.Core")
        {
            return null;
        }

        var dependencyPath = _assemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);
        if (dependencyPath != null)
        {
            return LoadFromAssemblyPath(dependencyPath);
        }

        return null;
    }
}