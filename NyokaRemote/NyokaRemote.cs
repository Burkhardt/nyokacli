using System;
using JsonPit;
using Persist;

namespace NyokaServerConfiguration
{
    public class NyokaRemoteInfo : JsonPit.Item
    {
        public string RepositoryServer { get; set; }
        public string ZementisServer { get; set; }
        public string ZementisModeler { get; set; } 
    }
}