using Aplication.Interfaces;
using domain.Entities;
using Infraesructure.Perssistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraesructure.Querys
{
    public class StatusQuery : IStatusQuery
    {
        private readonly AppDBContext _context;

        public StatusQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Status>> GetAllStatus()
        {
            var Status = await _context.statuses.ToListAsync();
            return Status;
        }
    }
}
