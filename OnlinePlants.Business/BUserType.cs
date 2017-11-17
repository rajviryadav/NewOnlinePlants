using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePlants.Model.BusinessModel;
using OnlinePlants.Data;


namespace OnlinePlants.Business
{
    public class BUserType
    {
        OnlinePlantsContext db = new OnlinePlantsContext();
        public List<UserType> GetUserTypeList()
        {
            return db.tblUserType.ToList();
        }

    }
}
