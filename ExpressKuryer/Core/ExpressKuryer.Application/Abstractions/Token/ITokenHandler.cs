using ExpressKuryer.Application.DTOs.Token;
using ExpressKuryer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        TokenDTO CreateAccessToken(AppUser user,int minute);
        List<Claim> CreateClaims(AppUser user);

    }
}
