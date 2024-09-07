using CarPark.Core.Models;
using CarPark.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.Business.Abstract
{
    public interface IPersonelService
    {
        GetManyResult<Personel> GetPersonelsByAge();
    }
}
