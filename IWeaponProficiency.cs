using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proficiencies
{
    public interface IWeaponProficiencyOnCharacter
    {
        void Apply(Character character, float original, ref float result);
    }
}
