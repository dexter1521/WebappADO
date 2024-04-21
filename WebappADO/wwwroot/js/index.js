const _modeloPersona = {
    idPersona: 0,
    nombreCompleto: "",
    idDepartamento: 0,
    sueldo: 0,
    fechaContrato: ""
}

function MostrarEmpleados() {

    fetch("/Home/listaPersonas")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {

                $("#tablaEmpleados tbody").html("");


                responseJson.forEach((persona) => {
                    $("#tablaEmpleados tbody").append(
                        $("<tr>").append(
                            $("<td>").text(persona.nombreCompleto),
                            $("<td>").text(persona.refDepartamento.nombre),
                            $("<td>").text(persona.sueldo),
                            $("<td>").text(persona.fechaContrato),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm boton-editar-empleado").text("Editar").data("dataEmpleado", persona),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 boton-eliminar-empleado").text("Eliminar").data("dataEmpleado", persona),
                            )
                        )
                    )
                })

            }


        })


}

document.addEventListener("DOMContentLoaded", function () {

    MostrarEmpleados();

    fetch("/Home/listaDepartamentos")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {

            if (responseJson.length > 0) {
                responseJson.forEach((item) => {

                    $("#cboDepartamento").append(
                        $("<option>").val(item.idDepartamento).text(item.nombre)
                    )

                })
            }

        })

    $("#txtFechaContrato").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    })


}, false)


function MostrarModal() {

    $("#txtNombreCompleto").val(_modeloPersona.nombreCompleto);
    $("#cboDepartamento").val(_modeloPersona.idDepartamento == 0 ? $("#cboDepartamento option:first").val() : _modeloPersona.idDepartamento)
    $("#txtSueldo").val(_modeloPersona.sueldo);
    $("#txtFechaContrato").val(_modeloPersona.fechaContrato)


    $("#modalEmpleado").modal("show");

}

$(document).on("click", ".boton-nuevo-empleado", function () {

    _modeloPersona.idPersona = 0;
    _modeloPersona.nombreCompleto = "";
    _modeloPersona.idDepartamento = 0;
    _modeloPersona.sueldo = 0;
    _modeloPersona.fechaContrato = "";

    MostrarModal();

})

$(document).on("click", ".boton-editar-empleado", function () {

    const _persona = $(this).data("dataEmpleado");


    _modeloPersona.idPersona = _persona.idPersona;
    _modeloPersona.nombreCompleto = _persona.nombreCompleto;
    _modeloPersona.idDepartamento = _persona.refDepartamento.idDepartamento;
    _modeloPersona.sueldo = _persona.sueldo;
    _modeloPersona.fechaContrato = _persona.fechaContrato;

    MostrarModal();

})

$(document).on("click", ".boton-guardar-cambios-empleado", function () {

    const modelo = {
        idPersona: _modeloPersona.idPersona,
        nombreCompleto: $("#txtNombreCompleto").val(),
        refDepartamento: {
            idDepartamento: $("#cboDepartamento").val()
        },
        sueldo: $("#txtSueldo").val(),
        fechaContrato: $("#txtFechaContrato").val()
    }


    if (_modeloPersona.idPersona == 0) {

        fetch("/Home/guardarPersona", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {

                if (responseJson.valor) {
                    $("#modalEmpleado").modal("hide");
                    Swal.fire("Listo!", "Empleado fue creado", "success");
                    MostrarEmpleados();
                }
                else
                    Swal.fire("Lo sentimos", "No se puedo crear", "error");
            })

    } else {

        fetch("/Home/editarPersona", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {

                if (responseJson.valor) {
                    $("#modalEmpleado").modal("hide");
                    Swal.fire("Listo!", "Empleado fue actualizado", "success");
                    MostrarEmpleados();
                }
                else
                    Swal.fire("Lo sentimos", "No se puedo actualizar", "error");
            })

    }


})


$(document).on("click", ".boton-eliminar-empleado", function () {

    const _persona = $(this).data("dataEmpleado");

    Swal.fire({
        title: "Esta seguro?",
        text: `Eliminar empleado "${_persona.nombreCompleto}"`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, volver"
    }).then((result) => {

        if (result.isConfirmed) {

            fetch(`/Home/eliminarPersona?idPersona=${_persona.idPersona}`, {
                method: "DELETE"
            })
                .then(response => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {

                    if (responseJson.valor) {
                        Swal.fire("Listo!", "Empleado fue elminado", "success");
                        MostrarEmpleados();
                    }
                    else
                        Swal.fire("Lo sentimos", "No se puedo eliminar", "error");
                })

        }



    })

})