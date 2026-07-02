using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using misaki.misakiCode.Extensions;
using BaseLib.Utils;
using misaki.misakiCode.Character;
using misaki.misakiCode.Powers;
namespace misaki.misakiCode.Relics;
[Pool(typeof(misakiRelicPool))]
public sealed class swordGust : misakiRelic
{           
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "sword_gust.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "sword_gust_outline.png".RelicImagePath();
    protected override string BigIconPath => "big/sword_gust.png".RelicImagePath();
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack)
        {
            Flash();
            await PowerCmd.Apply<swordGustPower>(choiceContext, Owner.Creature, 1, Owner.Creature, cardPlay.Card,
                false);
        }
        await base.AfterCardPlayed(choiceContext, cardPlay);
    }
}