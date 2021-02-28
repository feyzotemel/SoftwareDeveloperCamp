using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrate.EntityFramework
{  
    public class EfUserDal : EfEntityRepositoryBase<User, FinalProjectContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new FinalProjectContext())
            {
                ////context.OperationClaims hata verirse yukarıya using System.Linq; ekle 
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();

            }
        }
    }
}
