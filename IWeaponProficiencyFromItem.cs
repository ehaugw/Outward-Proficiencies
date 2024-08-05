using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proficiencies
{
    public interface IWeaponProficiencyFromItem
    {
        float Apply(Item item);
    }
}
