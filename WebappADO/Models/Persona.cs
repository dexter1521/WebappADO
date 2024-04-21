namespace WebappADO.Models
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string nombreCompleto { get; set; }
        
        public Departamento refDepartamento { get; set; }
        public string sueldo { get; set; }

        public string fechaContrato { get; set; }
    }
}
