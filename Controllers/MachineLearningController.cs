using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using spartan_claim_service.Models;

namespace spartan_claim_service.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MachineLearningController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> PostScore(ClaimsTest claim)
        {
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
                                        "InscClaimAmtReimbursed", claim.InscClaimAmtReimbursed.ToString()
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
                                        "IsDead", claim.IsDead.ToString()
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

                const string apiKey = "ePOngGFg8fv4ZmnIs3gqe4Af6qgs7o3z"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("http://1b2d133e-b0ab-4c1a-ba69-bea4ab34854e.eastus.azurecontainer.io/score");

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
                    Console.WriteLine("Result: {0}", result);

                    return Ok(new { result });
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    return BadRequest(new { status = "error", message = "error getting score" });
                }
            }
        }
    }
}
