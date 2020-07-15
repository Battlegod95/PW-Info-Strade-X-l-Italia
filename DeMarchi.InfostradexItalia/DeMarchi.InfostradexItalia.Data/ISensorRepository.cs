using DeMarchi.InfostradexItalia.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeMarchi.InfostradexItalia.Data
{
    public interface ISensorRepository : IRepositoryBase<Sensor>
    {
        IEnumerable<Sensor> GetLastSensor();
    }
}
