using System.Collections.Generic;

namespace LmsBeApp_Group06.Dtos
{
    public class PageDataDto<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set;}
    }
}
