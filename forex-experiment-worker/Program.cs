using System;
using System.Text;
using System.Text.Json;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using Dasync.Collections;

using forex_experiment_worker.Models;

namespace forex_experiment_worker
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static List<string> pairs = new List<string>()
        {
            "AUDUSD",
            "EURUSD",
            "GBPUSD",
            "NZDUSD",
            "USDCAD",
            "USDCHF",
            "USDJPY"
        };
        static async Task Main(string[] args)
        {
            Console.WriteLine("Test Secrets!");
            Console.WriteLine(Environment.GetEnvironmentVariable("mysecret"));

            string keyId = Environment.GetEnvironmentVariable("keyId");
            string key = Environment.GetEnvironmentVariable("key");
            var realtimeprices = new Dictionary<string,ForexPricesDTO>();
            
            
            var days = await GetDays(keyId,key,"forexdailyprices");

            await days.ParallelForEachAsync(async x => 
            {
                var pricesResult = await GetRealPrices(x.Pair,x.Datetime.ToString("yyyyMMdd"),"forexdailyrealprices");
                realtimeprices.Add(pricesResult.Item1,pricesResult.Item2);
            },maxDegreeOfParallelism: 8);

            foreach (var day in days)
            {
                Console.WriteLine(day.Pair + " " + day.Date);
            }

            await UploadFileToS3(keyId,key,"forexexperiments","Hello","world!!!!");
            //await UploadDailyPrices();
            /*
            var startDate = "20190324";
            var endDate = "20200522";
            
            foreach(var pair in pairs)
            {
                var url = $"http://localhost:5002/api/forexdailyprices/{pair}/{startDate}/{endDate}";
                var days = await GetAsync<List<ForexDailyPriceDTO>>(url);
                foreach (var day in days)
                {
                    //var dayString= JsonSerializer.Serialize<ForexDailyPriceDTO>(day);
                    //await UploadFileToS3("forexdailyprices",day.Pair+day.Date,dayString);
                    //Console.WriteLine(day.Pair+day.Date + " uploaded");

                    //var realPrices = await GetRealPrices(day.Pair,day.Datetime.ToString("yyyyMMdd"));

                    //await UploadFileToS3("forexdailyrealprices",realPrices.Item1,JsonSerializer.Serialize<ForexPricesDTO>(realPrices.Item2));
                    var prices = await ReadFileFromS3($"{pair}{day.Datetime.ToString("yyyyMMdd")}","forexdailyrealprices");
                    Console.WriteLine($"{pair} {prices.prices.Length}");
                    //Console.WriteLine(realPrices.Item1 + " uploaded");
                }
                
                
            }*/
           

        }

        public static async Task UploadDailyPrices()
        {
            var startDate = "20190324";
            var endDate = "20200522";
            
            foreach(var pair in pairs)
            {
                var url = $"http://localhost:5002/api/forexdailyprices/{pair}/{startDate}/{endDate}";
                var days = await GetAsync<List<ForexDailyPriceDTO>>(url);
                foreach (var day in days)
                {
                    var dayString= JsonSerializer.Serialize<ForexDailyPriceDTO>(day);
                    await UploadFileToS3(null,null,"forexdailyprices",day.Pair+day.Datetime.ToString("yyyyMMdd"),dayString);
                    Console.WriteLine(day.Pair+day.Date + " uploaded");

                }
                
                
            }
        }

        /*public static async Task UploadDailyRealPrices()
        {
            var startDate = "20190324";
            var endDate = "20200522";
            
            foreach(var pair in pairs)
            {
                var url = $"http://localhost:5002/api/forexdailyprices/{pair}/{startDate}/{endDate}";
                var days = await GetAsync<List<ForexDailyPriceDTO>>(url);
                foreach (var day in days)
                {
                    var realPrices = await GetRealPrices(day.Pair,day.Datetime.ToString("yyyyMMdd"));

                    await UploadFileToS3(null,null,"forexdailyrealprices",realPrices.Item1,JsonSerializer.Serialize<ForexPricesDTO>(realPrices.Item2));
                  
                    Console.WriteLine(realPrices.Item1 + " uploaded");

                }
                
                
            }
        }*/


        public static async Task<List<ForexDailyPriceDTO>> GetDays(string awskeyId,string awskey, string bucketname)
        {
             var days = new List<ForexDailyPriceDTO>();
             var dayskey = new List<string>();
             using (var client = string.IsNullOrEmpty(awskeyId) ? new AmazonS3Client(RegionEndpoint.USEast1) : new AmazonS3Client(awskeyId,awskey,RegionEndpoint.USEast1))
             {
                 ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucketname,
                    MaxKeys = 10
                };


                ListObjectsV2Response response;
                do
                {
                    response = await client.ListObjectsV2Async(request);

                    // Process the response.
                    foreach (S3Object entry in response.S3Objects)
                    {
                        //Console.WriteLine("key = {0} size = {1}",
                         //   entry.Key, entry.Size);
                        //var dailyprice = await ReadFileFromS3<ForexDailyPriceDTO>(entry.Key,bucketname); 
                        dayskey.Add(entry.Key); 
                        Console.WriteLine(entry.Key);
                         
                    }
                    //Console.WriteLine("Next Continuation Token: {0}", response.NextContinuationToken);
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                await dayskey.ParallelForEachAsync(async x => 
                {
                   var dailyprice = await ReadFileFromS3<ForexDailyPriceDTO>(x,bucketname); 
                   days.Add(dailyprice);
                   Console.WriteLine(x + "Added");
                },maxDegreeOfParallelism: 8);

                
                return days;

             }
        }

        public static async Task<T> ReadFileFromS3<T>(string key,string bucket)
        {
            var responseBody = string.Empty;
            var request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = key
            };
            using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
            using (GetObjectResponse response = await client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                //string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                //string contentType = response.Headers["Content-Type"];
                //Console.WriteLine("Object metadata, Title: {0}", title);
                //Console.WriteLine("Content type: {0}", contentType);

                responseBody = reader.ReadToEnd(); // Now you process the response body.
                return JsonSerializer.Deserialize<T>(responseBody);
            }
            
        }

        public static async Task UploadFileToS3(string awskeyId,string awskey,
                            string bucketname,string key,string info)
        {
            using (var client = string.IsNullOrEmpty(awskeyId) ? new AmazonS3Client(RegionEndpoint.USEast1) : new AmazonS3Client(awskeyId,awskey,RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = new MemoryStream(Encoding.UTF8.GetBytes(info)),
                        Key = key,
                        BucketName = bucketname,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }
        }

        static async Task<(string,ForexPricesDTO)> GetRealPrices(string pair,string day,string bucketName)
        {
            //var urlgetdailyrealprices = $"http://localhost:5002/api/forexdailyrealprices/{pair}/{day}";
                    
            var dailyrealprices = await ReadFileFromS3<ForexPricesDTO>(pair+day,bucketName);
            Console.WriteLine($"reading {pair} {day}");

            return (pair+day,dailyrealprices);
        }

        static async Task<T> GetAsync<T>(string url)
        {
            var responseBody = await client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<T>(responseBody);
            return data;
        }

        static async Task<HttpResponseMessage> PatchAsync<T>(T dto,string url)
        {
            var stringPrice= JsonSerializer.Serialize<T>(dto);
            var stringPriceContent = new StringContent(stringPrice,UnicodeEncoding.UTF8,"application/json");
            var responsePriceBody = await client.PatchAsync(url,stringPriceContent);
            return responsePriceBody;
        }

        static async Task<HttpResponseMessage> PostAsync<T>(T dto,string url)
        {
            var stringPrice= JsonSerializer.Serialize<T>(dto);
            var stringPriceContent = new StringContent(stringPrice,UnicodeEncoding.UTF8,"application/json");
            var responsePriceBody = await client.PostAsync(url,stringPriceContent);
            return responsePriceBody;
        }
    }

    
}
