using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace spartan_claim_service.Models
{
    public partial class ClaimsTest
    {
        [JsonProperty("beneId")]
        public string BeneId { get; set; }
        [JsonProperty("provider")]
        public string Provider { get; set; }
        [JsonProperty("inscClaimAmtReimbursed")]

        public double InscClaimAmtReimbursed { get; set; }
        [JsonProperty("isInpatient")]

        public bool IsInpatient { get; set; }
        [JsonProperty("gender")]

        public byte Gender { get; set; }
        [JsonProperty("race")]

        public byte Race { get; set; }
        [JsonProperty("renalDiseaseIndicator")]

        public bool RenalDiseaseIndicator { get; set; }
        [JsonProperty("chronicCondAlzheimer")]

        public bool ChronicCondAlzheimer { get; set; }
        [JsonProperty("chronicCondHeartfailure")]

        public bool ChronicCondHeartfailure { get; set; }
        [JsonProperty("chronicCondKidneyDisease")]

        public bool ChronicCondKidneyDisease { get; set; }
        [JsonProperty("chronicCondCancer")]

        public bool ChronicCondCancer { get; set; }
        [JsonProperty("chronicCondObstrPulmonary")]

        public bool ChronicCondObstrPulmonary { get; set; }
        [JsonProperty("chronicCondDepression")]

        public bool ChronicCondDepression { get; set; }
        [JsonProperty("chronicCondDiabetes")]

        public bool ChronicCondDiabetes { get; set; }
        [JsonProperty("chronicCondIschemicHeart")]

        public bool ChronicCondIschemicHeart { get; set; }
        [JsonProperty("chronicCondOsteoporasis")]

        public bool ChronicCondOsteoporasis { get; set; }
        [JsonProperty("chronicCondRheumatoidarthritis")]
        public bool ChronicCondRheumatoidarthritis { get; set; }
        [JsonProperty("chronicCondStroke")]

        public bool ChronicCondStroke { get; set; }
        [JsonProperty("ipannualReimbursementAmt")]

        public double IpannualReimbursementAmt { get; set; }
        [JsonProperty("ipannualDeductibleAmt")]

        public double IpannualDeductibleAmt { get; set; }
        [JsonProperty("opannualReimbursementAmt")]

        public double OpannualReimbursementAmt { get; set; }
        [JsonProperty("opannualDeductibleAmt")]

        public double OpannualDeductibleAmt { get; set; }
        [JsonProperty("age")]

        public int Age { get; set; }
        [JsonProperty("isDead")]

        public double IsDead { get; set; }
        [JsonProperty("daysAdmitted")]

        public byte DaysAdmitted { get; set; }
        [JsonProperty("totalDiagnosis")]

        public double TotalDiagnosis { get; set; }
        [JsonProperty("totalProcedure")]

        public double TotalProcedure { get; set; }
        [JsonProperty("id")]

        public int Id { get; set; }
    }
}
