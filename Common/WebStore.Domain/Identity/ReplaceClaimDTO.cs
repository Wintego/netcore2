using System.Security.Claims;

namespace WebStore.Domain.Identity
{
    public class ReplaceClaimDTO : UserInfoDTO
    {
        public Claim OldClain { get; set; }
        public Claim NewClaim { get; set; }
    }
    
}
