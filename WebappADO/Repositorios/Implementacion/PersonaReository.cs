using WebappADO.Models;
using WebappADO.Repositorios.Contrato;
using System.Data;
using System.Data.SqlClient;

namespace WebappADO.Repositorios.Implementacion
{
    public class PersonaReository : IGenericRepository<Persona>
    {
        private readonly string _cadenaSQL = "";
        public PersonaReository(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }

        public async Task<List<Persona>> Lista()
        {
            List<Persona> _lista = new List<Persona>();

            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ListaPersonas", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        _lista.Add(new Persona
                        {
                            IdPersona = (int)dr["idPersona"],
                            nombreCompleto = dr["nombreCompleto"]?.ToString(),
                            refDepartamento = new Departamento()
                            {
                                IdDepartamento = (int)dr["idDepartamento"],
                                nombre = dr["nombre"]?.ToString()
                            },
                            sueldo = (string)dr["sueldo"],
                            fechaContrato = dr["fechaContrato"]?.ToString(),
                        });
                    }
                }
            }

            return _lista;
        }

        public async Task<bool> Guardar(Persona modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_guardarPersona", conexion);

                cmd.Parameters.AddWithValue("@NombreCompleto", modelo.nombreCompleto);
                cmd.Parameters.AddWithValue("@IdDepartamento", modelo.refDepartamento.IdDepartamento);
                cmd.Parameters.AddWithValue("@sueldo", modelo.sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", modelo.fechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas = await cmd.ExecuteNonQueryAsync();
                if (filas_afectadas > 0)
                {
                    return true;
                }
                else { return false; }
            }

        }

        public async Task<bool> Editar(Persona modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_editaPersona", conexion);

                cmd.Parameters.AddWithValue("@idPersona", modelo.IdPersona);
                cmd.Parameters.AddWithValue("@NombreCompleto", modelo.nombreCompleto);
                cmd.Parameters.AddWithValue("@IdDepartamento", modelo.refDepartamento.IdDepartamento);
                cmd.Parameters.AddWithValue("@sueldo", modelo.sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", modelo.fechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas = await cmd.ExecuteNonQueryAsync();
                if (filas_afectadas > 0)
                {
                    return true;
                }
                else { return false; }
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_eliminarPersona", conexion);
                cmd.Parameters.AddWithValue("@idPersona", id);
                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas = await cmd.ExecuteNonQueryAsync();
                if (filas_afectadas > 0)
                {
                    return true;
                }
                else { return false; }
            }
        }

    }
}
