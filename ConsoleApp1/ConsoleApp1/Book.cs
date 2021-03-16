using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Book
    {
        public BookInfo metadata { get; set; }
        public List<BookInfo> Files { get; set; }
    }
}
