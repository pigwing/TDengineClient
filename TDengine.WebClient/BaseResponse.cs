using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDengine.WebClient
{
    public abstract class BaseResponse
    {
        public int Code { get; set; }
        public int Rows { get; set; }
        [JsonProperty(PropertyName = "desc")]
        public string? Description { get; set; }
    }
}
