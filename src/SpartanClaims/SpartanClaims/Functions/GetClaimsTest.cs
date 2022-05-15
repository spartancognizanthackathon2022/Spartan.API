using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SpartanClaims.Infrastructure.Services;

namespace SpartanClaims.Functions
{
    public class GetClaimsTest
    {
        private readonly ClaimsDbContext _context;

        public GetClaimsTest(ClaimsDbContext context)
        {
            _context = context;
        }

        [Function("GetClaimsTest")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetClaimsTest/{id}")] HttpRequestData req,
            FunctionContext executionContext,
            int id)
        {
            var logger = executionContext.GetLogger("GetClaimsTest");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var result = await _context.ClaimsTests.FindAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync(JsonConvert.SerializeObject(result));

            if (result == null)
            {
                response = req.CreateResponse(HttpStatusCode.NotFound);

                return response;
            }

            return response;
        }
    }
}
