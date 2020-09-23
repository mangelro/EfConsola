using System;

using Autofac;

using Datos.Ef;
using Datos.Ef.Repositorios;

using EfConsola.Infraestructura;

using FundacionOlivar.DDD.SharedKernel;

using Microsoft.EntityFrameworkCore;

using Modelo.Ef;

namespace EfConsola
{
    internal static class Program
    {

        private static void Main(string[] args)
        {

            using (var scope = ConfigureAutofac().BeginLifetimeScope())
            {

                IUnitOfWork uow = scope.Resolve<IUnitOfWork>();

                //UnitOfWorkFactory uowFactory = scope.Resolve<UnitOfWorkFactory>();
                RepositoryFactory repoFactory = scope.Resolve<RepositoryFactory>();



                //CrearAutor(uow, repoFactory);
                //ConsultarAutor(repoFactory);
                ModificarAutor(uow,repoFactory);
                //CrearBlog(uow,repoFactory);
                //ConsultarBlog(uow, repoFactory);
                //LimpiarBlogPost(uow, repoFactory);
                //AddBlogPost(uow);
                //CrearProyecto(uow, repoFactory);
            }



            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadLine();
        }


        private static void CrearAutor(IUnitOfWork uow, RepositoryFactory factory)
        {
            IAutorRepository autorRepo = factory.GetInstance<IAutorRepository>(); //uow.GetRepository<IAutorRepository>();


            uow.Begin();

            var autor = Autor.NewAutor(new DateTime(1974,10,22));

            autor.EstablecerNombre("Miguel Á. Romera");
            autor.EstablecerDireccion(new Direccion("Merced alta n1 3C", "Jaén", "23002", "JAEN", "ESPAÑA"));


            autorRepo.Add(autor);

            uow.Commit();



        }


        private static void ConsultarAutor(RepositoryFactory factory)
        {

            IAutorRepository autorRepo = factory.GetInstance<IAutorRepository>();// uow.GetRepository<IAutorRepository>();

            //var autorA = autorRepo.GetById(AutorID.FromString("01e5fcfe-3a8c-4ffb-ad7a-8958d75d4f4a"));
            var autorA = autorRepo.GetById(Guid.Parse("1b35faac-3406-486c-9470-1138446548ee"));

            // uow.Commit();

            Console.WriteLine(autorA);
        }

        private static void ModificarAutor(IUnitOfWork uow, RepositoryFactory factory)
        {

            IAutorRepository autorRepo = factory.GetInstance<IAutorRepository>(); //uow.GetRepository<IAutorRepository>();
            var autor = autorRepo.GetById(Guid.Parse("1b35faac-3406-486c-9470-1138446548ee"));

            uow.Begin();

            //var autor = autorRepo.GetById(AutorID.FromString("42d8b4d3-ec22-4fd0-a7d8-2a1c653db1a7"));

            autor.EstablecerFechaNacimiento(new DateTimeOffset(1979,10,30,0,0,0,TimeSpan.Zero));

            autorRepo.Update(autor);
            uow.Commit();

        }


        private static void CrearBlog(IUnitOfWork uow, RepositoryFactory factory)
        {

            IAutorRepository autorRepo = factory.GetInstance<IAutorRepository>(); //uow.GetRepository<IAutorRepository>();
            IBlogRepository blogRepo = factory.GetInstance<IBlogRepository>(); //uow.GetRepository<IBlogRepository>();

            //var autor = autorRepo.GetById(AutorID.FromString("01e5fcfe-3a8c-4ffb-ad7a-8958d75d4f4a"));
            var autor = autorRepo.GetById(Guid.Parse("01e5fcfe-3a8c-4ffb-ad7a-8958d75d4f4a"));


            var blog = Blog.NewBlog();
            
            blog.EstablecerAutor(autor);
            
            blog.EstablecerUrl("http://www.oliva3.net");

            //var p1 = blog.AddPost("Titulo 1", "Este es el primer post");
            //p1.EstablecerValoracion(Valoracion.FromInteger(2));

            //var p2 = blog.AddPost("Titulo 2", "Este es el segundo post");
            //p2.EstablecerValoracion(Valoracion.FromInteger(5));

           // blog.AddPost("Titulo 2", "repetidos");

            uow.Begin();

            blogRepo.Add(blog);

            uow.Commit();
        }

        private static void ConsultarBlog(IUnitOfWork uow, RepositoryFactory factory)
        {
            IBlogRepository blogRepo = factory.GetInstance<IBlogRepository>(); //uow.GetRepository<IBlogRepository>();

            var blog = blogRepo.GetById(1);

            //blogRepo.LoadPosts(blog);

            Console.WriteLine(blog);

        }

        private static void LimpiarBlogPost(IUnitOfWork uow, RepositoryFactory factory)
        {
            IBlogRepository blogRepo = factory.GetInstance<IBlogRepository>(); //uow.GetRepository<IBlogRepository>();


            uow.Begin();

            var blog = blogRepo.GetById(1);

            blogRepo.LoadPosts(blog);

            blog.ClearPost();

            uow.Commit();

            Console.WriteLine(blog);

        }



        private static void AddBlogPost(IUnitOfWork uow, RepositoryFactory factory)
        {

            IBlogRepository blogRepo = factory.GetInstance<IBlogRepository>(); //uow.GetRepository<IBlogRepository>();

            var blog = blogRepo.GetById(1);

            blog.AddPost("Titulo 5", "Este es el segundo post");



            uow.Commit();

        }

        private static void CrearProyecto(IUnitOfWork unitOfWork, RepositoryFactory repoFactory)
        {
            IProyectoRepository proyectoRepo = repoFactory.GetInstance<IProyectoRepository>();  //uow.GetRepository<IProyectoRepository>();



            unitOfWork.Begin();

            var pro = Proyecto.NewProyecto(ProyectoCode.FromString("cod-12"), Guid.NewGuid());

            pro.EstablecerNombre("proyecto pepito");
            pro.EstablecerFechaFinalizacion(DateTimeOffset.Now);

            proyectoRepo.Add(pro);

            unitOfWork.Commit();

        }






















        private static IContainer ConfigureAutofac()
        {

            var builder = new ContainerBuilder();

            builder
               .RegisterAssemblyTypes(typeof(EfAutorRepository).Assembly)
               .Where(s => s.FullName.EndsWith("Repository"))
               .AsImplementedInterfaces();



            builder.Register<RepositoryFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return (t => c.Resolve(t));
            }).SingleInstance();


            builder.RegisterType<UoWconfig>()
                .As<IUoWConfig>();


            builder.RegisterType<EfUoW>()
                .As<IUnitOfWork>()
                .As<DbContext>()
                .AsSelf()
                .SingleInstance(); //No me gusta 


            builder.Register<UnitOfWorkFactory>(ctx =>
            {

                //var c = ctx.Resolve<IComponentContext>();
                //return (() => c.Resolve<IUnitOfWork>());

                /*
                 * Es importante que se cree uno nuevo por casa petición de servicio,
                 * ya que cada contexto (Task) necesita uno distinto
                 */
                IDomainEventPublisher publisher = ctx.Resolve<IDomainEventPublisher>();
                IUoWConfig config = ctx.Resolve<IUoWConfig>();

                return () => new EfUoW(config, publisher);
            }).InstancePerLifetimeScope();


            builder.RegisterType<FakeEventPublisher>()
                .As<IDomainEventPublisher>()
                .InstancePerLifetimeScope();



            return builder.Build();
        }


    }



}
