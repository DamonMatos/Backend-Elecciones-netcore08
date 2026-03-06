using WsElecciones.Application.DTOs.PagoAsbanc;
using WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.CuentasCorriente;
using Mapster;

namespace WsElecciones.Application.Map;

public class MapperConfiguration
{
    public static void Configure()
    {
        //TypeAdapterConfig<CreateCuentaCorrienteView, DTOs.CuentasCorriente.GetCuentaCorrienteDTO.GetCuentaCorrienteRequestDTO>.NewConfig();
        //TypeAdapterConfig<CreateCuentaCorrienteDTO.CuentaCorrienteCuotaDTO, CuentaCorrienteView.CuentaCorrienteCuota>.NewConfig();
        TypeAdapterConfig<CreatePagoAsbancDTO.CreatePagoAsbancRequestDTO, PagoAsbanc>.NewConfig();
        TypeAdapterConfig<PagoAsbanc, CreatePagoAsbancDTO.PagoAsbancDTO>.NewConfig();
        TypeAdapterConfig<CreateProgramacionCuentaCorrienteDTO.FacultadCarreraDTO, CreateProgramacionCuentaCorriente.FacultadCarrera>.NewConfig();
        TypeAdapterConfig<CreateProgramacionCuentaCorrienteDTO.CreateProgramacionCuentaCorrienteRequestDTO, CreateProgramacionCuentaCorriente>.NewConfig()
            .Map(dest => dest.CodDepartamentos, src => src.CodDepartamentos ?? Array.Empty<string>())
            .Map(dest => dest.CodCampus, src => src.CodCampus ?? Array.Empty<string>())
            .Map(dest => dest.FacultadCarreras, src => src.FacultadCarreras == null
                ? Array.Empty<CreateProgramacionCuentaCorriente.FacultadCarrera>()
                : src.FacultadCarreras.Select(x => x.Adapt<CreateProgramacionCuentaCorriente.FacultadCarrera>()).ToArray())
            .Map(dest => dest.CodAlumnos, src => src.CodAlumnos ?? Array.Empty<string>())
            .Map(dest => dest.CodAlumnosExcluir, src => src.CodAlumnosExcluir ?? Array.Empty<string>())
            
            ;
    }
}
