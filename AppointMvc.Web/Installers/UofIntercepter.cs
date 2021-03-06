﻿using Appoint.EntityFramework;
using Appoint.EntityFramework.DbContextProvider;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointMvc.Web.Installers
{
    public class UofIntercepter : IInterceptor
    {

        public IDbContextProvider<App_DbContext> _provider { get; set; }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                _provider.Release();
            }
        }
        
    }
}