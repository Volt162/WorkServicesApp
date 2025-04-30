using System;
using MopsterTeams.Enums;
using Newtonsoft.Json;

namespace MopsterTeams.Models
{
    public class ProfileModel
    {
        [JsonProperty("employeeId")]
        public string EmployeeId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobilePhoneNumber")]
        public string MobilePhoneNumber { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("language")]
        public ELanguage Language { get; set; }
    }

    public class Address
    {
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("houseNumber")]
        public string HouseNumber { get; set; }

        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("poBox")]
        public string PoBox { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}
