using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.Pcf.GivingToCustomer.Core.Abstractions.Gateways
{
    public interface IPreferencesFromDistributedCache
    {
        Task<List<Preference>> GetAllAsync();
        Task<Preference> GetByIdAsync(Guid preferenceId);
    }
}
