using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SpartanClaims.Infrastructure.Services;
using SpartanClaims.Models;

namespace SpartanClaims.Functions
{
    public class PutClaimsTest
    {
        private readonly ClaimsDbContext _context;

        public PutClaimsTest(ClaimsDbContext context)
        {
            _context = context;
        }

        [Function("PutClaimsTest")]
        public async Task<HttpResponseData> PutClaimsTestRun([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ClaimsTests/{id}")] HttpRequestData req,
            FunctionContext executionContext,
            int id)
        {
            var logger = executionContext.GetLogger("PutClaimsTest");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ClaimsTest claimsTest = JsonConvert.DeserializeObject<ClaimsTest>(requestBody);

            logger.LogInformation(requestBody);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            if (id != claimsTest.Id)
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);

                return response;
            }

            _context.Entry(claimsTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimsTestExists(id))
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);

                    return response;
                }
                else
                {
                    throw;
                }
            }

            response = req.CreateResponse(HttpStatusCode.NoContent);

            return response;
        }

        private bool ClaimsTestExists(int id)
        {
            return _context.ClaimsTests.Any(e => e.Id == id);
        }
    }
}
