/*****************************************************************
Filename:       CsvRow.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    For cvs file read

Revision log:
* 2017-02-21: Created
******************************************************************/
using System.Collections.Generic;

namespace KioskCheckoutSystem
{
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }
}
