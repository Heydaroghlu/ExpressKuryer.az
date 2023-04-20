using ExpressKuryer.Application.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        TokenDTO CreateAccessToken(int minute);

    }
}
