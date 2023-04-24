using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDengine.WebClient
{
    public class TDengineMeta
    {
        public string? Name { get; set; }
        public TDengineDataType DataType { get; set; }
        public int Length { get; set; }
    }

    public enum TDengineDataType
    {
        Timestamp,
        Int,
        IntUnsigned,
        BigInt,
        BigIntUnsigned,
        Float,
        Double,
        Binary,
        SmallInt,
        SmallIntUnsigned,
        TinyInt,
        TinyIntUnsigned,
        Bool,
        NChart,
        Json,
        VarChar
    }
}
