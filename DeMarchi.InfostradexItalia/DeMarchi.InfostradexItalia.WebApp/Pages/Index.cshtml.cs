using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeMarchi.InfostradexItalia.Data;
using DeMarchi.InfostradexItalia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DeMarchi.InfostradexItalia.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISensorRepository _sensorRepository;
        private readonly ITrafficRepository _trafficRepository;

        public IEnumerable<Sensor> Sensor { get; set; }
        public IEnumerable<Traffic> Traffic { get; set; }
        public IEnumerable<Sensor> LastSensor { get; set; }
        public IEnumerable<Traffic> TrafficState { get; set; }
        public IEnumerable<Traffic> ConteggioChart { get; set; }
        public IEnumerable<Traffic> AutomobileChart { get; set; }
        public IEnumerable<Traffic> CamionChart { get; set; }
        public IEnumerable<Traffic> MotocicloChart { get; set; }

        public IndexModel(ILogger<IndexModel> logger,
                          ISensorRepository sensorRepository,
                          ITrafficRepository trafficRepository)
        {
            _logger = logger;
            _sensorRepository = sensorRepository;
            _trafficRepository = trafficRepository;
        }

        public void OnGet()
        {
            Sensor = _sensorRepository.GetAll();
            Traffic = _trafficRepository.GetAll();
            LastSensor = _sensorRepository.GetLastSensor();
            TrafficState = _trafficRepository.GetState();
            ConteggioChart = _trafficRepository.GetConteggio();
            AutomobileChart = _trafficRepository.GetAutomobili();
            CamionChart = _trafficRepository.GetCamion();
            MotocicloChart = _trafficRepository.GetMotociclo();
        }
    }
}
