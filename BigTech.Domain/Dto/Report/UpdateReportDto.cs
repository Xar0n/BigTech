using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigTech.Domain.Dto.Report;
public record class UpdateReportDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
