using System;
using System.Collections.Generic;
using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Service
{
    public class BaseService
    {
        protected readonly HelpSGFContext _context;

        public BaseService( HelpSGFContext context)
        {
            _context = context;
        }
    }
}
