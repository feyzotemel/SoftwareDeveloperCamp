using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Castle.DynamicProxy;
using Business.Constants;
using Core.Extensions;

namespace Business.BusinessAspects.Autofac
{
    //JWT için bu securedOperation

    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            //GetService kırmızı olursa business e nuget paket olarak 
            //Microsoft.Extensions.DependencyInjection kur ve yukarıya elinle 
            //"using Microsoft.Extensions.DependencyInjection;" ekle lamba çıkmıyor.
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
