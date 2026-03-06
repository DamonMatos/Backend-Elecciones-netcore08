using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Entitites;
using WsElecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WsElecciones.Persistence.Context;

public partial class CuentaCorrienteContext : DbContext
{
    public CuentaCorrienteContext()
    {
    }

    public CuentaCorrienteContext(DbContextOptions<CuentaCorrienteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CuentaCorriente> CuentaCorrientes { get; set; }

    public virtual DbSet<CuentaCorrienteCuotum> CuentaCorrienteCuota { get; set; }
    public virtual DbSet<PagoAsbanc> PagoAsbanc { get; set; }

    public virtual DbSet<AppUser> Usuario { get; set; }

    public virtual DbSet<AppElecciones> Elecciones { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CuentaCorriente>(entity =>
        {
            entity.HasKey(e => e.CodCuentaCorriente).HasName("CUENTA_CORRIENTE_PK");

            entity.ToTable("CUENTA_CORRIENTE");

            entity.Property(e => e.CodCuentaCorriente).HasColumnName("cod_cuenta_corriente");
            entity.Property(e => e.Activo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("AC")
                .IsFixedLength()
                .HasColumnName("activo");
            entity.Property(e => e.CodAlumno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_alumno");
            entity.Property(e => e.CodCampus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_campus");
            entity.Property(e => e.CodCarrera)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_carrera");
            entity.Property(e => e.CodCategoriaPagoDetallePeriodo).HasColumnName("cod_categoria_pago_detalle_periodo");
            entity.Property(e => e.CodDepartamento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_departamento");
            entity.Property(e => e.CodEmpresa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_empresa");
            entity.Property(e => e.CodNivel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_nivel");
            entity.Property(e => e.CodPeriodoAcademico)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_periodo_academico");
            entity.Property(e => e.CodPeriodoAcademicoReferencia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_periodo_academico_referencia");
            entity.Property(e => e.CodPersona).HasColumnName("cod_persona");
            entity.Property(e => e.CodTasaCuotas).HasColumnName("cod_tasa_cuotas");
            entity.Property(e => e.CodTipoDocumento).HasColumnName("cod_tipo_documento");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.IdSis1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_sis1");
            entity.Property(e => e.IdSis2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_sis2");
            entity.Property(e => e.IdSis3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_sis3");
            entity.Property(e => e.IdSis4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_sis4");
            entity.Property(e => e.IdSis5)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_sis5");
            entity.Property(e => e.TipoAlumno)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipo_alumno");
            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_modificacion");
            entity.Property(e => e.VigenteCuentaCorriente)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("SI")
                .IsFixedLength()
                .HasColumnName("vigente_cuenta_corriente");
        });

        modelBuilder.Entity<CuentaCorrienteCuotum>(entity =>
        {
            entity.HasKey(e => e.CodCuentaCorrienteCuota).HasName("CUENTA_CORRIENTE_CUOTA_PK");

            entity.ToTable("CUENTA_CORRIENTE_CUOTA");

            entity.Property(e => e.CodCuentaCorrienteCuota).HasColumnName("cod_cuenta_corriente_cuota");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CantidadCreditos)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("cantidad_creditos");
            entity.Property(e => e.CantidadCursos)
                .HasDefaultValue(0)
                .HasColumnName("cantidad_cursos");
            entity.Property(e => e.CodConcepto).HasColumnName("cod_concepto");
            entity.Property(e => e.CodCuentaCorriente).HasColumnName("cod_cuenta_corriente");
            entity.Property(e => e.CodMoneda).HasColumnName("cod_moneda");
            entity.Property(e => e.CodNivel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cod_nivel");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaProrroga)
                .HasColumnType("datetime")
                .HasColumnName("fecha_prorroga");
            entity.Property(e => e.FechaVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_vencimiento");
            entity.Property(e => e.Importe)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("importe");
            entity.Property(e => e.ImporteCancelado)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("importe_cancelado");
            entity.Property(e => e.ImporteImpuesto)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("importe_impuesto");
            entity.Property(e => e.Item).HasColumnName("item");
            entity.Property(e => e.NumeroCuota).HasColumnName("numero_cuota");
            entity.Property(e => e.TipoCambio)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("tipo_cambio");
            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_modificacion");
            entity.Property(e => e.VigenteCuentaCorrienteCuota)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("SI")
                .IsFixedLength()
                .HasColumnName("vigente_cuenta_corriente_cuota");

            entity.HasOne(d => d.CodCuentaCorrienteNavigation).WithMany(p => p.CuentaCorrienteCuota)
                .HasForeignKey(d => d.CodCuentaCorriente)
                .HasConstraintName("CUENTA_CORRIENTE_CUOTA_FK4");
        });

        modelBuilder.Entity<PagoAsbanc>(entity =>
        {
            entity.ToTable("Pago_Asbanc", "ctacte");

            entity.HasKey(e => e.CodPagoAsbanc)
                  .HasName("PK_Pago_Asbanc");

            entity.Property(e => e.CodPagoAsbanc)
                  .HasColumnName("cod_pago_asbanc")
                  .ValueGeneratedOnAdd();

            entity.Property(e => e.FechaTransaccion)
                  .HasColumnName("fecha_transaccion")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CanalPago)
                  .HasColumnName("canal_pago")
                  .HasMaxLength(3)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.CodigoBanco)
                  .HasColumnName("codigo_banco")
                  .HasMaxLength(5)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.NumOperacionBanco)
                  .HasColumnName("num_operacion_banco")
                  .HasMaxLength(13)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.FormaPago)
                  .HasColumnName("forma_pago")
                  .HasMaxLength(3)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.TipoConsulta)
                  .HasColumnName("tipo_consulta")
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.IdConsulta)
                  .HasColumnName("id_consulta")
                  .HasMaxLength(15)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.CodigoProducto)
                  .HasColumnName("codigo_producto")
                  .HasMaxLength(3)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.NumDocumento)
                  .HasColumnName("num_documento")
                  .HasMaxLength(16)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.ImportePagado)
                  .HasColumnName("importe_pagado")
                  .HasColumnType("money")
                  .IsRequired();

            entity.Property(e => e.MonedaDoc)
                  .HasColumnName("moneda_doc")
                  .HasMaxLength(2)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.Procesado)
                  .HasColumnName("procesado")
                  .HasMaxLength(2)
                  .IsUnicode(false)
                  .HasDefaultValue("NO")
                  .IsRequired();

            entity.Property(e => e.FechaProceso)
                  .HasColumnName("fecha_proceso")
                  .HasColumnType("datetime");

            entity.Property(e => e.FechaCreacion)
                  .HasColumnName("fecha_creacion")
                  .HasColumnType("datetime")
                  .IsRequired();
        });

        modelBuilder.Ignore<AppUser>();

        modelBuilder.Ignore<AppElecciones>();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}