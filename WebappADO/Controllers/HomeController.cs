using WebappADO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebappADO.Repositorios.Contrato;

namespace WebappADO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<Departamento> _departamentoRepository;
        private readonly IGenericRepository<Persona> _personaRepository;
        

        public HomeController(ILogger<HomeController> logger, 
            IGenericRepository<Persona> personaRepository, 
            IGenericRepository<Departamento> departamentoRepository)
        {
            _logger = logger;
            _personaRepository = personaRepository;
            _departamentoRepository = departamentoRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> listaDepartamentos()
        {
            List<Departamento> _lista = await _departamentoRepository.Lista();
            return StatusCode(StatusCodes.Status200OK, _lista);
            
        }

        [HttpGet]
        public async Task<IActionResult> listaPersonas()
        {
            List<Persona> _lista = await _personaRepository.Lista();
            return StatusCode(StatusCodes.Status200OK, _lista);

        }

        [HttpPost]
        public async Task<IActionResult> guardarPersona([FromBody] Persona modelo )
        {
            bool _respuesta = await _personaRepository.Guardar(modelo);
            if (_respuesta)
            {
                return StatusCode(StatusCodes.Status200OK, new { valor = _respuesta, message = "Registro guardado" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _respuesta, message = "Error al guardar" });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> editarPersona([FromBody] Persona modelo)
        {
            bool _respuesta = await _personaRepository.Editar(modelo);
            if (_respuesta)
            {
                return StatusCode(StatusCodes.Status200OK, new { valor = _respuesta, message = "Registro actualizado correctamente" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _respuesta, message = "Error al guardar" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> eliminarPersona(int idPersona)
        {
            bool _respuesta = await _personaRepository.Eliminar(idPersona);
            if (_respuesta)
            {
                return StatusCode(StatusCodes.Status200OK, new { valor = _respuesta, message = "Registro actualizado correctamente" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _respuesta, message = "Error al guardar" });
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
