using System;
using System.Collections.Generic;
using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Services
{
    public class UsersService : BaseService
    {
        public UsersService(HelpSGFContext context) : base(context) { }

    }
}
