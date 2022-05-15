using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpartanClaims.Configurations;
using SpartanClaims.Models;

namespace SpartanClaims.Functions
{
    public class PostMachineLearningScore
    {
        public readonly ClaimsConfigurations claimsConfig;

        public PostMachineLearningScore(IOptions<ClaimsConfigurations> claimsOptions)
        {
            claimsConfig = claimsOptions.Value;
        }

        [Function("PostMachineLearningScore")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "MachineLearning")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PostMachineLearningScore");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var machineLearningResponse = req.CreateResponse(HttpStatusCode.OK);
            machineLearningResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ClaimsTest claim = JsonConvert.DeserializeObject<ClaimsTest>(requestBody);

            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            using (var client = new HttpClient(handler))
            {
                // Request data goes here
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>()
                    {
                        {
                            "data",
                            new List<Dictionary<string, string>>()
                            {
                                new Dictionary<string, string>()
                                {
                                    {
                                        "Provider", claim.Provider.ToString()
                                    },
                                                                        {
                                        "InscClaimAmtReimbursed", claim.InscClaimAmtReimbursed.ToString()
                                    },
                                    {
                                        "AttendingPhysician", claim.AttendingPhysician.ToString()
                                    },
                                    {
                                        "is_inpatient", claim.IsInpatient ? "1" : "0"
                                    },
                                    {
                                        "Gender", claim.Gender.ToString()
                                    },
                                    {
                                        "Race", claim.Race.ToString()
                                    },
                                                                        {
                                        "RenalDiseaseIndicator", claim.RenalDiseaseIndicator ? "1" : "0"
                                    },
                                    {
                                        "State", claim.State.ToString()
                                    },
                                    {
                                        "County", claim.County.ToString()
                                    },
                                    {
                                        "ChronicCond_Alzheimer", claim.ChronicCondAlzheimer ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_Heartfailure", claim.ChronicCondHeartfailure ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_KidneyDisease", claim.ChronicCondKidneyDisease ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_Cancer", claim.ChronicCondCancer ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_ObstrPulmonary", claim.ChronicCondObstrPulmonary ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_Depression", claim.ChronicCondDepression ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_Diabetes", claim.ChronicCondDiabetes ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_IschemicHeart", claim.ChronicCondIschemicHeart ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_Osteoporasis", claim.ChronicCondOsteoporasis ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_rheumatoidarthritis", claim.ChronicCondRheumatoidarthritis ? "1" : "0"
                                    },
                                    {
                                        "ChronicCond_stroke", claim.ChronicCondStroke ? "1" : "0"
                                    },
                                    {
                                        "IPAnnualReimbursementAmt", claim.IpannualReimbursementAmt.ToString()
                                    },
                                    {
                                        "IPAnnualDeductibleAmt", claim.IpannualDeductibleAmt.ToString()
                                    },
                                    {
                                        "OPAnnualReimbursementAmt", claim.OpannualReimbursementAmt.ToString()
                                    },
                                    {
                                        "OPAnnualDeductibleAmt", claim.OpannualDeductibleAmt.ToString()
                                    },
                                    {
                                        "age", claim.Age.ToString()
                                    },
                                    {
                                        "Is_Dead", claim.IsDead ? "1" : "0"
                                    },
                                    {
                                        "DaysAdmitted", claim.DaysAdmitted.ToString()
                                    },
                                    {
                                        "TotalDiagnosis", claim.TotalDiagnosis.ToString()
                                    },
                                    {
                                        "TotalProcedure", claim.TotalProcedure.ToString()
                                    }
                                }
                            }
                        }
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                        {
                            "method", "predict"
                        }
                    }
                };

                string apiKey = claimsConfig.ApiKey; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("http://1317a3f1-2793-4bb0-83ce-f1dfe9e4056c.eastus.azurecontainer.io/score");

                var requestString = JsonConvert.SerializeObject(scoreRequest);
                var content = new StringContent(requestString);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)
                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {

                    string result = await response.Content.ReadAsStringAsync();
                    Result result_integer = JsonConvert.DeserializeObject<Result>(result);
                    Console.WriteLine("Result: {0}", result_integer.Results[0]);

                    machineLearningResponse = req.CreateResponse(HttpStatusCode.OK);
                    machineLearningResponse.WriteString(JsonConvert.SerializeObject(result_integer.Results[0]));

                    return machineLearningResponse;
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);

                    machineLearningResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    machineLearningResponse.WriteString(JsonConvert.SerializeObject(new { status = "error", message = "error getting score" }));

                    return machineLearningResponse;
                }
            }
        }
    }
}
