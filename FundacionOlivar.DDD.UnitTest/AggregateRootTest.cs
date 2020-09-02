using System;
using System.Security.Cryptography.X509Certificates;

using FundacionOlivar.DDD.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FundacionOlivar.DDD.UnitTest
{
    [TestClass]
    public class AggregateRootTest
    {
        [TestMethod]
        public void Clase_Root()
        {

           

            

        }



        public class Usuario: AggregateRoot<Guid>
        {

            public Usuario(Guid id)
            {
                Identity = id;
            }

            public string Nombre { get; set; }
        }


    }
}
