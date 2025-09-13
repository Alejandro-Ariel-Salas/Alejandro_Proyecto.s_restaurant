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
    public class DeliveryTypeQuery : IDeliveryTypeQuery
    {
        private readonly AppDBContext _context;

        public DeliveryTypeQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<DeliveryType>> GetAllDeliveryTypes()
        {
            var deliveryTypes = await _context.deliveryTypes.ToListAsync();
            return deliveryTypes;
        }
    }
}
