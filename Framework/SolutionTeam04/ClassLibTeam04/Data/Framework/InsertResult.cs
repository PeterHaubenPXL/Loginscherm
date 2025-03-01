using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam04.Data.Framework
{
    public class InsertResult : BaseResult
    {
        //NewId wordt teruggegeven door SQL Server - Auto Increment
        public int NewId { get; set; }
    }
}
