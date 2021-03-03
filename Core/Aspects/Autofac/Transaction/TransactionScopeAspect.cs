using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
        //TransactionScopeAspect : kullanıldığı tüm kodları trycach içine almış gibi hareket eder.
        //Yarım kalan bir tranaction varsa önceki yapılan veritabanı transactionunu geri alır.
        //Örnek Api de ProductManager içindeki AddTransactionalTest
        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (System.Exception e)
                {
                    transactionScope.Dispose();
                    throw e;
                }
            }
        }
    }
}
