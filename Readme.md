# Flitbit.Data.Cluster.Redis

FlitBit Data clustered memory implementation using Redis for the backend.

---
As of FlitBit.Data version 3.2.1, the base DataModelRepository implementation supports transaction aware caching.

The repository level caching is supported via two interfaces, `IClusteredMemory` which holds cached copies of data models, 
and `IClusterNotifier` which allows processes to monitor cache related notifications (pub-sub).