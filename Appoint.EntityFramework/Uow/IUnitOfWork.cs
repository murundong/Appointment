using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Uow
{
    public interface IUnitOfWork
    {
        int SaveChange();

    }
}
