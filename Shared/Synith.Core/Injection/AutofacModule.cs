using Autofac;
using Synith.Caching;

namespace Synith.Core.Injection;
public class AutofacModule : Module
{
    private readonly string _baseNamespace;

    public AutofacModule(string serviceName)
    {
        _baseNamespace = $"{nameof(Synith)}.{serviceName}";
    }

    protected override void Load(ContainerBuilder builder)
    {
        RegisterScoped(builder, $"{nameof(Synith)}.Security",
            new List<string> { "Services" });
        RegisterScoped(builder, $"{_baseNamespace}.Application",
            new List<string> { "Services", "Validators" });

        builder.RegisterType<Cache>().As<ICache>().InstancePerLifetimeScope();
    }

    private static void RegisterScoped(ContainerBuilder builder, string assemblyName, IEnumerable<string> namespaces)
    {
        IEnumerable<string> finalNamespaces = namespaces.Select(ns => $"{assemblyName}.{ns}");

        builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load(assemblyName))
            .Where(t => finalNamespaces.Contains(t.Namespace))
            .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == $"I{t.Name}")!)
            .InstancePerLifetimeScope();
    }
}
