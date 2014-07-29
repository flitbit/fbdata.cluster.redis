using FlitBit.IoC;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using AssemblyWireup = FlitBit.Data.Cluster.Redis.AssemblyWireup;

[assembly: Wireup(typeof(AssemblyWireup))]

namespace FlitBit.Data.Cluster.Redis
{
  /// <summary>
  ///   Wires up this assembly.
  /// </summary>
  public sealed class AssemblyWireup : IWireupCommand
  {
    /// <summary>
    ///   Called by the wireup framework when this assembly is wired.
    /// </summary>
    /// <param name="coordinator"></param>
    public void Execute(IWireupCoordinator coordinator)
    {
      Container.Root
        .ForType<IClusterNotifications>()
               .Register((c, p) => c.New<IClusteredMemory>() as IClusterNotifications)
               .ResolveAsSingleton()
               .End();

      Container.Root
        .ForType<IClusterNotifier>()
               .Register((c, p) => c.New<IClusteredMemory>() as IClusterNotifier)
               .ResolveAsSingleton()
               .End();
    }
  }
}