using DeMarchi.InfostradexItalia.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeMarchi.InfostradexItalia.Data
{
    public interface ITrafficRepository : IRepositoryBase<Traffic>
    {
        IEnumerable<Traffic> GetState();
        IEnumerable<Traffic> GetConteggio();
        IEnumerable<Traffic> GetAutomobili();
        IEnumerable<Traffic> GetCamion();
        IEnumerable<Traffic> GetMotociclo();
    }
}
