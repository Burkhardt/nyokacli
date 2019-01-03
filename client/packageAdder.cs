using System.Collections.Generic;
using Constants;
using FSOpsNS;
using NetworkUtilsNS;
using System.IO;

// @TODO make sure all streams are being closed properly?
namespace PackageManagerNS {

    public static class PackageManager {
        public static void initDirectories() {
            FSOps.createCodeDataModelDirs(logExisting: true, logCreated: true, logError: true);
        }
        
        public static void addPackage(ResourceType resourceType, string resourceName) {
            FSOps.createCodeDataModelDirs();
            System.Console.WriteLine($"Adding {resourceType.ToString().ToLower()} resource \"{resourceName}\"");

            if (FSOps.resourceExists(resourceType, resourceName)) {
                System.Console.WriteLine($"{resourceType} resource \"{resourceName}\" already exists");
                return;
            }

            try {
                using (FileStream fileStream = FSOps.createResourceFile(resourceType, resourceName))
                using (Stream resourceStream = NetworkUtils.getResource(resourceType, resourceName))
                {
                    resourceStream.CopyTo(fileStream);
                }
            } catch (NetworkUtils.NetworkUtilsException e) {
                System.Console.WriteLine("Error: " + e.Message);
            }
        }

        public static void removePackage(ResourceType resourceType, string resourceName) {
            FSOps.createCodeDataModelDirs();

            System.Console.WriteLine($"Removing {resourceType.ToString().ToLower()} resource \"{resourceName}\"");
            if (!FSOps.resourceExists(resourceType, resourceName)) {
                System.Console.WriteLine($"{resourceType} resource \"{resourceName}\" does not exist");
                return;
            }

            FSOps.removeResource(resourceType, resourceName);
        }

        public static void listResources(ResourceType? listType) {
            if (!FSOps.hasCodeDataModelDirs()) {
                System.Console.WriteLine($"Missing resource directories. Try running {ConstStrings.APPLICATION_ALIAS} init?");
                return;
            }

            List<ResourceType> resourcesToList = listType.HasValue ?
                new List<ResourceType> { listType.Value } :
                new List<ResourceType> { ResourceType.code, ResourceType.data, ResourceType.model };


            foreach (ResourceType resourceType in resourcesToList) {
                System.Console.WriteLine($"{resourceType.ToString()}:");
                foreach (string resourceName in FSOps.resourceNames(resourceType)) {
                    System.Console.WriteLine("    " + resourceName);
                }
            }
        }
    }
}