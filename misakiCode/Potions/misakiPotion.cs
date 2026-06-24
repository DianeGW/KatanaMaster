using BaseLib.Abstracts;
using BaseLib.Utils;
using misaki.misakiCode.Character;

namespace misaki.misakiCode.Potions;

[Pool(typeof(misakiPotionPool))]
public abstract class misakiPotion : CustomPotionModel;