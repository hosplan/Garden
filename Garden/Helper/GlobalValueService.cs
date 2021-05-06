using Garden.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Garden.Helper
{
    public class GlobalValueService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;

        public GlobalValueService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        
        public Dictionary<string, string> GardenRoleName
        {
            get
            {
                Dictionary<string, string> role_Dic = new Dictionary<string, string>();
                var role_list = _context.Roles.ToList();
                
                foreach(var role in role_list)
                {
                    role_Dic.Add(role.Id, role.Name);
                }

                return role_Dic;
            }
            
        }

        public string loginUserName
        {
            get
            {
                return _context.Users.FirstOrDefault(user => user.Id == loginUserId).UserName;
            }
        }
    }
}
