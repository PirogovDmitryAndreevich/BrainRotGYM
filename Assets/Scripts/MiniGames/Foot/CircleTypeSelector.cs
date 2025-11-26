using UnityEngine;

public class CircleTypeSelector : MonoBehaviour
{
    // Базовые вероятности (в процентах)
    private float baseScoreChance = 50f;     
    private float baseTimeChance = 10f;      
    private float baseMinusScoreChance = 20f; 
    private float baseMinusTimeChance = 20f;

    [SerializeField] private float scoreChance;
    [SerializeField] private float timeChance;
    [SerializeField] private float minusScoreChance;
    [SerializeField] private float minusTimeChance;

    // Коэффициент усложнения — насколько быстро падают положительные шары
    private float difficultyScale = 0.5f;

    public CircleType GetCircleType(float timeElapsed, float maxDifficultyTime)
    {
        float difficulty = Mathf.Clamp01(timeElapsed / maxDifficultyTime);

        // Чем больше difficulty  тем ниже бонусные вероятности
        scoreChance = baseScoreChance * (1f - difficulty * difficultyScale);
        timeChance = baseTimeChance * (1f - difficulty * difficultyScale);

        // Чем больше difficulty  тем чаще минусовые шары
        minusScoreChance = baseMinusScoreChance + baseScoreChance * difficulty * difficultyScale;
        minusTimeChance = baseMinusTimeChance + baseTimeChance * difficulty * difficultyScale;

        // Сумма вероятностей
        float total = scoreChance + timeChance + minusScoreChance + minusTimeChance;

        // Нормализация (чтобы сумма была = 1)
        scoreChance /= total;
        timeChance /= total;
        minusScoreChance /= total;
        minusTimeChance /= total;

        // Выпадение
        float r = Random.value;

        if (r < scoreChance)
            return CircleType.Score;

        if (r < scoreChance + timeChance)
            return CircleType.Time;

        if (r < scoreChance + timeChance + minusScoreChance)
            return CircleType.MinusScore;

        return CircleType.MinusTime;
    }
}
