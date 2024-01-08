using System.Text;

namespace TDengine.WebClient
{
    public class TDengineResponse : BaseResponse
    {
        
        [System.Text.Json.Serialization.JsonPropertyName("column_meta")]
        public List<object[]>? ColumnMeta { get; set; }
        public List<object[]>? Data { get; set; }
        public List<TDengineMeta>? ColumnTDengineMeta
        {
            get
            {
                if(Code  != 0) return null;

                if(ColumnMeta == null) return null;

                List<TDengineMeta> list = new List<TDengineMeta>();
                foreach (object[] objects in ColumnMeta)
                {
                    string dataType = RemoveWhitespacesUsingLinq(objects[1].ToString()!);
                    int.TryParse(objects[2].ToString()!, out int length);
                    Enum.TryParse(dataType, true, out TDengineDataType tdDengineDataType);
                    list.Add(new TDengineMeta()
                    {
                        DataType = tdDengineDataType,
                        Length = length,
                        Name = objects[0].ToString()
                    });
                }
                return list;
            }
        }

        private string RemoveWhitespacesUsingLinq(string source)
        {
            return new string(source.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
    
}
