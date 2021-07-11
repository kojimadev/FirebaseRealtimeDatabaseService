using System;
using System.IO;
using System.Threading.Tasks;
using FirebaseRealtimeDatabaseService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FirebaseFunctions
{
    public static class Functions
    {
        /// <summary>
        /// Firebaseからデータを取得するサンプルAPI
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("GetRecords")]
        public static async Task<IActionResult> GetRecords(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Firebaseからデータを取得する
	        var service = GetFirebaseService();
            var records = await service.GetRecordsAsync<TestRecord>("items/");

            // 取得したデータを返す
            string responseMessage = string.Empty;
            foreach (var record in records)
            {
	            responseMessage += record.ToString();
            }

            return new OkObjectResult(responseMessage);
        }

        /// <summary>
        /// Firebaseにデータを登録するサンプルAPI
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("UpdateRecord")]
        public static async Task<IActionResult> UpdateRecord(
	        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
	        ILogger log)
        {
            // クエリかリクエストボディにnameパラメータがあれば取得する
	        string name = req.Query["name"];
	        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
	        dynamic data = JsonConvert.DeserializeObject(requestBody);
	        name ??= data?.name;

            // 登録用のオブジェクトを作成する
	        var record = new TestRecord() { Id = Guid.NewGuid().ToString(), IntValue = 10, StringValue = name };

            // Firebaseにデータを登録
	        var service = GetFirebaseService();
	        await service.UpdateRecordAsync("items/", record.Id, record);

            // 登録したデータを返す
	        string responseMessage = $"UpdateRecord:{record}";
	        return new OkObjectResult(responseMessage);
        }

        /// <summary>
        /// Firebaseのデータにアクセスするサービスを取得する
        /// </summary>
        /// <returns></returns>
        private static IFirebaseService GetFirebaseService()
        {
            var factory = new FirebaseServiceFactory();
            return factory.GetFirebaseService("your secret", "your databaseUrl");
        }
    }
}
