using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IStatusQuery
    {
        public Task<List<Status>> GetAllStatus();
        public Task<bool> ExistEstatus(int id);
    }
}
