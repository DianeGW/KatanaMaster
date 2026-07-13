using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace misaki.misakiCode.Character;



public static class MisakiCardKeyWords
{
    [CustomEnum("Aura")]
    [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Aura;
    [CustomEnum("Cut")]
    [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Cut;
}