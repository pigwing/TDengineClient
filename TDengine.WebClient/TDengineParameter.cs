using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDengine.WebClient
{
    public class TDengineParameter
    {
        public string? Name { get; set; }
        public object? Value { get; set; }
        public TDengineDataType? DataType { get; set; }
    }
}
