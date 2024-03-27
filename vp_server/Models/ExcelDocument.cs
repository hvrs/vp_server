using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class ExcelDocument
{
    public int Id { get; set; }

    public byte[] DocExcel { get; set; } = null!;
}
