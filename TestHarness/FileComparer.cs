using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestHarness
{
    class FileComparer
    {
        public static bool Compare(string fileName1, string fileName2)
        {

            var file1 = new StreamReader(fileName1);
            var file2 = new StreamReader(fileName2);

            var line1 = file1.ReadLine();
            var line2 = file2.ReadLine();
            while (line1 != null)
            {
                if (line1 != line2)
                {
                    file1.Close();
                    file2.Close();
                    return false;
                }

                line1 = file1.ReadLine();
                line2 = file2.ReadLine();
            }

            file1.Close();
            file2.Close();

            if (line2 == null)
                return true;
            else
                return false;
        }
    }
}
