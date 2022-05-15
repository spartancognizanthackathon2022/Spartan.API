using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SpartanClaims.Infrastructure.Services;

namespace SpartanClaims.Functions
{
    public class DeleteClaimsTest
    {
        private readonly ClaimsDbContext _context;

        public DeleteClaimsTest(ClaimsDbContext context)
        {
            _context = context;
        }

        [Function("DeleteClaimsTest")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "ClaimsTests/{id}")] HttpRequestData req,
            FunctionContext executionContext,
            int id)
        {
            var logger = executionContext.GetLogger("DeleteClaimsTest");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var claimsTest = await _context.ClaimsTests.FindAsync(id);
            if (claimsTest == null)
            {
                response = req.CreateResponse(HttpStatusCode.NotFound);

                return response;
            }

            _context.ClaimsTests.Remove(claimsTest);
            await _context.SaveChangesAsync();

            response = req.CreateResponse(HttpStatusCode.NoContent);

            return response;
        }
    }
}
