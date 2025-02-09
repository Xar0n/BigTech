using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigTech.Domain.Dto;
public class TokenDto
{
    public string AcessToken { get; set; }

    public string RefreshToken { get; set; }
}
