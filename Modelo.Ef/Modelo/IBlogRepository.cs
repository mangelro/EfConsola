
using FundacionOlivar.DDD.SharedKernel;

namespace Modelo.Ef
{
    public interface IBlogRepository : IRepository<Blog, int>
    {
        void LoadPosts(Blog blog);
    }
}
