    using Ninject.Modules;
using DataAccessLayer;
using DataAccessLayer.Dapper;
using DataAccessLayer.EF;
using Model;

namespace BusinessLogic
{
    public class SimpleConfigModule : NinjectModule
    {
        private readonly bool _useDapper;

        public SimpleConfigModule(bool useDapper = false)
        {
            _useDapper = useDapper;
        }

        public override void Load()
        {
            if (_useDapper)
            {
                Bind<IRepository<Student>>().To<DapperRepository<Student>>().InSingletonScope();
                Bind<IRepository>().To<DapperRepository>().InSingletonScope();
            }
            else
            {
                Bind<IRepository<Student>>().To<EntityRepository<Student>>().InSingletonScope();
                Bind<IRepository>().To<EntityRepository>().InSingletonScope();
            }
        }
    }
}
