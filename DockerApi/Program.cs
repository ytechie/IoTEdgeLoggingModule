using System;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace dockerlogs
{
    class Program
    {
        public static DockerClient _client;
        public static bool useStorage = true;

        static async Task Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            }

            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(
                new ContainersListParameters(){
                    Limit = 10,
                });

            ContainerListResponse edgeAgent = null;
            List<ContainerListResponse> edgeModules = new List<ContainerListResponse>();

            foreach(var container in containers)
            {
                foreach(var label in container.Labels)
                {
                    if (label.Key.Contains("net.azure-devices.edge.owner"))
                    {
                        edgeModules.Add(container);
                    }
                }

                // edge case due to bug (no pun intended)
                if (container.Names.First() == "/edgeAgent")
                {
                    edgeAgent = container;
                    break;
                }
                Console.WriteLine($"{container.ID}");
            }
            
            TelemetryClient telemetry = new TelemetryClient();
            telemetry.InstrumentationKey = "";

            string containerName = "iotlogs";
            string blobName =  string.Format("{0}-{1}{2}", edgeAgent.Names.First().TrimStart('/'), DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss"), ".log");
            string accountName = "";
            string keyValue = "";

            StorageCredentials creds = new StorageCredentials(accountName, keyValue);
            CloudStorageAccount account = new CloudStorageAccount(creds, true);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);

            try
            {
                BlobRequestOptions requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
                await blobContainer.CreateIfNotExistsAsync(requestOptions, null);
            }
            catch (StorageException)
            {
                Console.WriteLine("If you are running with the default connection string, please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            CloudAppendBlob blockBlob = blobContainer.GetAppendBlobReference(blobName);

            ContainerLogsParameters logsParameters = new ContainerLogsParameters() { ShowStderr = true, ShowStdout = true, Follow = true };
            Stream logs = await _client.Containers.GetContainerLogsAsync(edgeAgent.ID, logsParameters, CancellationToken.None);

            List<string> logdata = new List<string>();

            using (StreamReader reader = new StreamReader(logs))
            {
                string line;
                while (true) 
                {
                    line = await reader.ReadLineAsync();

                    if (!string.IsNullOrEmpty(line))
                    {
                        logdata.Add(line);
                        Console.WriteLine(line);
                    }

                    if (logdata.Count > 500)
                    {
                        if (useStorage)
                        {
                            Console.WriteLine("Uploading...");
                            string data = string.Join(System.Environment.NewLine, logdata);
                            await blockBlob.CreateOrReplaceAsync();

                            try
                            {
                                await blockBlob.AppendTextAsync(data);
                            }
                            catch (System.Exception up)
                            {
                                throw up;
                            }

                            logdata.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Uploading...");

                            try
                            {
                                foreach (var msg in logdata)
                                {
                                    telemetry.TrackTrace(msg);
                                }
                            }
                            catch (System.Exception up)
                            {
                                throw up;
                            }

                            logdata.Clear();
                        }
                    }
                }
            }
        }
    }
}
