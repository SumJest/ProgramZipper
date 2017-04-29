using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramZipper
{
    public class InstallingOption
    {
        string[] extensions;
        string exefile;
        string extdeficon;

        public string[] Extensions { get { return extensions; } set { extensions = value; } }
        public string EXEFile { get { return exefile; } set { exefile = value; } }
        public string ExtDefaultIcon { get { return extdeficon; } set { extdeficon = value; } }

        public InstallingOption(string[] extensions, string exefile, string extdeficon)
        {
            this.extensions = extensions;
            this.exefile = exefile;
            this.extdeficon = extdeficon;
        }

    }
}
