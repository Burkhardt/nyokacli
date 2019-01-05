using System.Net;
using System.IO;
using PackageManagerNS;
using Constants;
using System.Collections.Generic;
using Newtonsoft.Json;
using InfoTransferContainers;

namespace NetworkUtilsNS
{
    public static class NetworkUtils
    {
        public class NetworkUtilsException : System.Exception
        {
            public NetworkUtilsException(string mssg) 
            : base(mssg)
            {
            }
        }
        private static readonly string resourcesUrl = "http://localhost:5000/api/resources";

        public static Stream getResource(ResourceType resourceType, string resourceName)
        {
            string url = $"{resourcesUrl}/{resourceType.ToString()}/{resourceName}";
            
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                try
                {
                    Stream stream = client.GetStreamAsync(url).Result;
                    return stream;
                }
                catch (System.Exception)
                {
                    throw new NetworkUtilsException("Unable to get requested resource from server");
                }
            }
        }

        public static DepsTransferContainer getResourceDeps(ResourceType resourceType, string resourceName, string version)
        {
            string url = $"{resourcesUrl}/{resourceType.ToString()}/{resourceName}/{version}/dependencies";

            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                try
                {
                    string resourceJson = client.GetStringAsync(url).Result;
                    DepsTransferContainer dependencies = JsonConvert.DeserializeObject<DepsTransferContainer>(resourceJson);
                    return dependencies;
                }
                catch (JsonReaderException)
                {
                    throw new NetworkUtilsException("Unable to process server response");
                }
                catch (System.Exception)
                {
                    throw new NetworkUtilsException("Unable to get requested information from server");
                }
            }
        }

        public static Dictionary<string, FileInfoTransferContainer> getAvailableResources(ResourceType resourceType)
        {
            string url = $"{resourcesUrl}/{resourceType.ToString()}";

            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                try
                {
                    string jsonArr = client.GetStringAsync(url).Result;
                    Dictionary<string, FileInfoTransferContainer> resources = JsonConvert.DeserializeObject<Dictionary<string, FileInfoTransferContainer>>(jsonArr);
                    return resources;
                }
                catch (JsonReaderException)
                {
                    throw new NetworkUtilsException("Unable to process server response");
                }
                catch (System.Exception)
                {
                    throw new NetworkUtilsException("Unable to get requested information from server");
                } 
            }
        }
    }
}