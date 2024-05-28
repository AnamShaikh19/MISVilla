using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IUnitofWork
    {
        IVillaRepositary Villa { get; }
        IVillaNumberRepositary VillaNumber { get; }
        IAmenityRepositary Amenity { get; }

        void save();
    }
}

