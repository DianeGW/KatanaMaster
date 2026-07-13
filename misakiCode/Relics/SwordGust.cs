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
public sealed class SwordGust : misakiRelic
{           
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "sword_gust.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "sword_gust_outline.png".RelicImagePath();
    protected override string BigIconPath => "big/sword_gust.png".RelicImagePath();
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack)
        {
            await PowerCmd.Apply<SwordGustPower>(choiceContext, Owner.Creature, 1, Owner.Creature, cardPlay.Card,
                true);
        }
        await base.AfterCardPlayed(choiceContext, cardPlay);
    }
}