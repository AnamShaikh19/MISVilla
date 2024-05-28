using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.Infrastracture.Repositary
{
    public class UnitOfWork : IUnitofWork
    {
        private readonly ApplicationDbContext _db;

        public IVillaRepositary Villa { get; private set; }
        public IVillaNumberRepositary VillaNumber { get; private set; }
        public IAmenityRepositary Amenity { get; private set; } 

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Villa = new VillaRepositary(_db);
            VillaNumber = new VillaNumberRepositary(_db);
            Amenity = new AmenityRepositary(_db);

        }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
