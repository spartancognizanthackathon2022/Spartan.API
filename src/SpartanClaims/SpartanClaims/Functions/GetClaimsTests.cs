using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SpartanClaims.Infrastructure.Services;

namespace SpartanClaims.Functions
{
    public class GetClaimsTests
    {
        private readonly ClaimsDbContext _context;

        public GetClaimsTests(ClaimsDbContext context)
        {
            _context = context;
        }

        [Function("GetClaimTests")]
        public async Task<HttpResponseData> GetClaimsTest([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetClaimTests");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var result = await _context.ClaimsTests.OrderByDescending(u => u.Id)
                                 .Where(x => x.State > 0)
                                  .Take(10).ToListAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync(JsonConvert.SerializeObject(result));

            return response;
        }
    }
}
