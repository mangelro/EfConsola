/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 31/07/2020 13:14:27
 *
 */

using System;
using System.Collections.Generic;

using System.Text;


namespace Datos.Ef.Excepciones
{
    /// <summary>
    /// EntidadNoEncontrada
    /// </summary>
    public class EntidadNoEncontrada:Exception
    {
        public EntidadNoEncontrada() : base()
        {
        }

        public EntidadNoEncontrada(string message) : base(message)
        {
        }

        public EntidadNoEncontrada(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}