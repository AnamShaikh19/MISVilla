using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.Infrastracture.Repositary
{
    public class VillaRepositary : Repositary<Villa>, IVillaRepositary
    {
        private readonly ApplicationDbContext _db;
        public VillaRepositary(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }



        public void Update(Villa entity)
        {
            _db.Villas.Update(entity);
        }

    }
}
