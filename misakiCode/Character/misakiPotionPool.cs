using BaseLib.Abstracts;
using misaki.misakiCode.Extensions;
using Godot;

namespace misaki.misakiCode.Character;

public class misakiPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => MisakiCharacter.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}