using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_infostrade.Data;
using WebApp_Infostrade.Models;

namespace WebApp_Infostrade.Views.Home
{
    public class IndexModel : PageModel
    {
        private readonly ISensorRepository _sensorRepository;

        public IEnumerable<Sensor> Sensor { get; set; }

        public IndexModel(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }
        public void OnGet()
        {
            Sensor = _sensorRepository.GetAll();
        }
    }
}
