using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;

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
