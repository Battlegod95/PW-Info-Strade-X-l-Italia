using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeMarchi.InfostradexItalia.Data;
using DeMarchi.InfostradexItalia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeMarchi.InfostradexItalia.WebApp.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly ITrafficRepository _trafficRepository;

        public IEnumerable<Sensor> Sensor { get; set; }
        public IEnumerable<Traffic> Traffic { get; set; }

        public IndexModel(ISensorRepository sensorRepository,
                          ITrafficRepository trafficRepository)
        {
            _sensorRepository = sensorRepository;
            _trafficRepository = trafficRepository;
        }

        public void OnGet()
        {
            Sensor = _sensorRepository.GetAll();
            Traffic = _trafficRepository.GetAll();
        }
    }
}