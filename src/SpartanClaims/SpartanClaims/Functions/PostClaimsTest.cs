using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SpartanClaims.Infrastructure.Services;
using SpartanClaims.Models;

namespace SpartanClaims.Functions
{
    public class PostClaimsTest
    {
        private readonly ClaimsDbContext _context;

        public PostClaimsTest(ClaimsDbContext context)
        {
            _context = context;
        }

        [Function("PostClaimsTest")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PostClaimsTest");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ClaimsTest claimsTest = JsonConvert.DeserializeObject<ClaimsTest>(requestBody);

            _context.ClaimsTests.Add(claimsTest);
            await _context.SaveChangesAsync();

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync(JsonConvert.SerializeObject(claimsTest));

            return response;
        }
    }
}
