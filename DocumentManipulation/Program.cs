using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentManipulation
{
    class Program
    {

        static string srcfilename = @"F:\proj\doc.docx";
        static string tarfilename = @"F:\proj\tar.docx";
        static void Main(string[] args)
        {
            DocManp d = new DocManp();
            d.ReadDoc(srcfilename);
        }
    }
}
