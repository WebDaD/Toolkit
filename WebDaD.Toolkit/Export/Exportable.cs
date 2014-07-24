using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Export
{
    public interface Exportable
    {
        /// <summary>
        /// Create a Content Object to send to Export
        /// </summary>
        /// <returns>A Content Object to use for Export</returns>
        Content ToContent();

        /// <summary>
        /// The Name of the Data as shown to the User
        /// </summary>
        string DataName();

        /// <summary>
        /// The Name of the File to create
        /// </summary>
        string Filename();
    }
}
