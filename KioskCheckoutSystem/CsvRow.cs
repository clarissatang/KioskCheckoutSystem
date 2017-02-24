/*****************************************************************
Filename:       CsvRow.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    For cvs file read

Revision log:
* 2017-02-21: Created
******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskCheckoutSystem
{
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }
}
