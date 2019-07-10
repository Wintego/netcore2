using System.Collections.Generic;
using System.Security.Claims;

namespace WebStore.Domain.Identity
{
    public abstract class ClaimInfoDTO : UserInfoDTO
    {
        public IEnumerable<Claim> Claim { get; set; }
    }
    
}
