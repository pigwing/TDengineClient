using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TDengine.WebClient.Model
{
    public class TDengineExecuteNonQueryResult : BaseResponse
    {
        [JsonProperty(PropertyName = "affected_rows")]
        public int AffectedRows { get; set; }
    }
}
