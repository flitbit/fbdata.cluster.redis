using System.Configuration;

namespace FlitBit.Data.Cluster.Redis.Config
{
	/// <summary>
	/// Configuration section for configuring the redis cluster used for distributed memory.
	/// </summary>
	public class RedisClusteredMemoryConfigSection : ConfigurationSection
	{
		internal const string SectionName = "flitbit.data.cluster.redis";
    const string PropertyNameOptions = "options";
    const string PropertyNameMemoryDbNum = "memoryDb";
    const string PropertyDataNotificationChannelName = "dataNotificationChannelName";
    
    const int DefaultMemoryDb = 0;
    const string DefaultDataNotificationChannelName = "flitbit.data.cluster";

    
    internal static RedisClusteredMemoryConfigSection Instance
		{
			get
			{
				var config = ConfigurationManager.GetSection(SectionName)
          as RedisClusteredMemoryConfigSection;
        return config ?? new RedisClusteredMemoryConfigSection();
			}
		}

    [ConfigurationProperty(PropertyNameMemoryDbNum, DefaultValue = DefaultMemoryDb)]
    public int MemoryDb
    {
      get { return (int)this[PropertyNameMemoryDbNum]; }
      set { this[PropertyNameMemoryDbNum] = value; }
	  }

    [ConfigurationProperty(PropertyNameOptions, IsRequired = true)]
    public string Options
    {
      get { return (string)this[PropertyNameOptions]; }
      set { this[PropertyNameOptions] = value; }
    }

    [ConfigurationProperty(PropertyDataNotificationChannelName, DefaultValue = DefaultDataNotificationChannelName)]
    public string DataNotificationChannelName
    {
      get { return (string)this[PropertyDataNotificationChannelName]; }
      set { this[PropertyDataNotificationChannelName] = value; }
    }
  }
}
