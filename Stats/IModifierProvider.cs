using System.Collections.Generic;

namespace RPG.Stats
{
public interface IModifierProvider 
{
    IEnumerable<float> GetadditiveModifier(Stat stat); // Enumurator yerine enumerable kullandık cünkü Enumurator icinde foreach loop kullanılamıyor.
}
}
