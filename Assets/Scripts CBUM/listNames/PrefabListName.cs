using UnityEngine;

public enum PrefabListName
{
    barraDeVida,
    rollsound,
    enemyBasicDeathSound,
    destroyEnemyParticle,
    impactoPositivo,
    hitSound,
    projetilDeAgua,
    tapaCollider,
    spawnerParticles,
    particulaDoCaraAparecendo,
    particulaPositiva,
    rockDestroyedSound,
    pegueiMaisMana,
    particulaPositivaMana,
    pegueiMaisVida,
    projetilDeGosma,
    gritoDoMonstro,
    enfaseDoBoss,
    clawCollider,
    gritoDeGolpeDoBoss,
    enfaseDoAtkEspecial,
    teletransporte,
    raioDeAtaque,
    ataqueEmCirculoPuloAlto,
    ataqueEmCirculoPuloBaixo,
    ataqueEmCirculoPuloRolar,
    poeiraDoRolar

}

public class PrefabList
{
    public static Object Load(PrefabListName name)
    {
        return Resources.Load(name.ToString());
    }
}