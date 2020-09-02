/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 8:34:37
 *
 */

using System;
using System.Collections.Generic;

using System.Text;

using EfConsola.Modelo;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EfConsola.Datos.Configuracion
{
    /// <summary>
    /// ConversionHelper
    /// </summary>
    public static class ConversionHelper
    {
        private readonly static string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";


        public static ValueConverter<Valoracion, int>  ValoracionConverter = new ValueConverter<Valoracion, int>(
            v => (int)v,
            v => Valoracion.FromInteger(v));


        public static ValueConverter<DateTimeOffset, string> DateTimeOffsetConverter = new ValueConverter<DateTimeOffset, string>(
            v => v.ToString(DATETIME_FORMAT),
            v => DateTimeOffset.ParseExact(v, DATETIME_FORMAT, System.Globalization.CultureInfo.InvariantCulture));


        public static ValueConverter<AutorID, string> AutorIDConverter = new ValueConverter<AutorID, string>(
         v => v.ToString(),
         v => AutorID.FromString(v));


    }
}