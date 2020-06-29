using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinaX.Services
{
    public interface ILocalizationService : IBuiltInService
    {
        IEnumerable<SystemLanguage> GetCurrentLanguages();
    }
}
