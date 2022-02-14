using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.Repositories
{
    public class TxtRepository
    {
        public string[] GetStringArrayFromTxt(string path) => File.ReadAllLines(path);
    }
}
