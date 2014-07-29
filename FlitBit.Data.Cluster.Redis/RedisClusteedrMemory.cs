using FlitBit.Data.Cluster.Redis.Config;
using FlitBit.IoC;
using FlitBit.IoC.Meta;
using StackExchange.Redis;
using System;

namespace FlitBit.Data.Cluster.Redis
{
  [ContainerRegister(typeof(IClusteredMemory), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
  public class RedisClusteedrMemory : ClusterNotifications, IClusteredMemory, IClusterNotifier
  {
    readonly ConnectionMultiplexer _conn;
    readonly IDatabase _db;
    readonly ISubscriber _subscriber;
    readonly string _publicationChannelName;

    public RedisClusteedrMemory()
    {
      var config = RedisClusteredMemoryConfigSection.Instance;

      var options = ConfigurationOptions.Parse(config.Options);
      _publicationChannelName = config.DataNotificationChannelName;
      _conn = ConnectionMultiplexer.Connect(options);
      _subscriber = _conn.GetSubscriber();
      _subscriber.Multiplexer.PreserveAsyncOrder = false;
      _subscriber.Subscribe(_publicationChannelName, HandleNotification);
      _db = _conn.GetDatabase(config.MemoryDb);
    }

    /// <summary>
    /// When overridden in a derived class, disposes any resources associated
    /// with the current instance. Subclasses must call the base implementation.
    /// </summary>
    /// <param name="disposing"></param>
    protected override bool PerformDispose(bool disposing)
    {
      _subscriber.Unsubscribe(_publicationChannelName, null, CommandFlags.FireAndForget);
      _conn.Dispose();
      return true;
    }

    void HandleNotification(RedisChannel ch, RedisValue topic) { PerformNotification(this, topic); }

    public bool TryGet(string key, out byte[] value)
    {
      var res = _db.StringGet(key);
      if (!res.IsNull)
      {
        value = res;
        return true;
      }
      value = null;
      return false;
    }

    public void Put(string key, byte[] value)
    {
      _db.StringSet(key, value, null, When.Always, CommandFlags.FireAndForget);
    }

    public void PutWithExpiration(string key, TimeSpan maxAge, byte[] value)
    {
      _db.StringSet(key, value, maxAge, When.Always, CommandFlags.FireAndForget);
    }

    public void Delete(string key) { _db.KeyDelete(key); }

    /// <summary>
    /// Publishes an activity notification.
    /// </summary>
    /// <param name="channel">name of the channel</param>
    /// <param name="identity">object identifier</param>
    /// <param name="observation">the observation</param>
    public void PublishNotification(string channel, string identity, string observation)
    {
      string notification = FormatNotification(channel, identity, observation);
      _subscriber.Publish(_publicationChannelName, notification, CommandFlags.FireAndForget);
    }
  }

}
